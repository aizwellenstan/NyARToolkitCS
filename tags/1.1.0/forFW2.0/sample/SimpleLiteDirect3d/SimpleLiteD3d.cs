﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using NyARToolkitCSUtils.Capture;
using NyARToolkitCSUtils.Raster;
using NyARToolkitCSUtils.Direct3d;
using NyARToolkitCSUtils.NyAR;
using jp.nyatla.nyartoolkit.cs;
using jp.nyatla.nyartoolkit.cs.core;
using jp.nyatla.nyartoolkit.cs.raster;
using jp.nyatla.nyartoolkit.cs.detector;

namespace SimpleLiteDirect3d
{

    public partial class SimpleLiteD3d : IDisposable, CaptureListener
    {
        private const String AR_CODE_FILE = "../../../../../data/patt.hiro";
        private const String AR_CAMERA_FILE = "../../../../../data/camera_para.dat";
        //DirectShowからのキャプチャ
        private CaptureDevice  m_cap;
        //NyAR
        private D3dSingleDetectMarker m_ar;
        private DsXRGB32Raster m_raster;
        //背景テクスチャ
        private NyARTexture_XRGB32 m_texture;
        /// Direct3D デバイス
        private Device _device = null;
        private Sprite _sprite = null;
        // 頂点バッファ/インデックスバッファ/インデックスバッファの各頂点番号配列
        private VertexBuffer _vertexBuffer = null;
        private IndexBuffer _indexBuffer = null;
        private static Int16[] _vertexIndices = new Int16[] { 2, 0, 1, 1, 3, 2, 4, 0, 2, 2, 6, 4, 5, 1, 0, 0, 4, 5, 7, 3, 1, 1, 5, 7, 6, 2, 3, 3, 7, 6, 4, 6, 7, 7, 5, 4 };
        /* 非同期イベントハンドラ
         * CaptureDeviceからのイベントをハンドリングして、バッファとテクスチャを更新する。
         */
        public void OnBuffer(CaptureDevice i_sender, double i_sample_time, IntPtr i_buffer, int i_buffer_len)
        {
            int w = i_sender.video_width;
            int h = i_sender.video_height;
            int s = w * (i_sender.video_bit_count / 8);
            
            //テクスチャにRGBを取り込み()
            lock (this)
            {
                //カメラ映像をARのバッファにコピー
                this.m_raster.setBuffer(i_buffer);

                //テクスチャ内容を更新
                this.m_texture.CopyFromXRGB32(this.m_raster);
            }
            return;
        }
        /* キャプチャを開始する関数
         */
        public void StartCap()
        {
            this.m_cap.StartCapture();
        }
        /* キャプチャを停止する関数
         */
        public void StopCap()
        {
            this.m_cap.StopCapture();
        }


        /* Direct3Dデバイスを準備する関数
         */
        private Device PrepareD3dDevice(Control i_window)
        {
            PresentParameters pp = new PresentParameters();

            // ウインドウモードなら true、フルスクリーンモードなら false を指定
            pp.Windowed = true;
            // スワップとりあえずDiscardを指定。
            pp.SwapEffect = SwapEffect.Discard;
            CreateFlags fl_base = CreateFlags.FpuPreserve;

            try{
                return new Device(0, DeviceType.Hardware, i_window.Handle, fl_base|CreateFlags.HardwareVertexProcessing, pp);
            }catch (Exception ex1){
                Debug.WriteLine(ex1.ToString());
                try{
                    return new Device(0, DeviceType.Hardware, i_window.Handle, fl_base | CreateFlags.SoftwareVertexProcessing, pp);
                }catch (Exception ex2){
                    // 作成に失敗
                    Debug.WriteLine(ex2.ToString());
                    try{
                        return new Device(0, DeviceType.Reference, i_window.Handle, fl_base | CreateFlags.SoftwareVertexProcessing, pp);
                    }catch (Exception ex3){
                        throw ex3;
                    }
                }
            }
        }
        public bool InitializeApplication(Form1 topLevelForm,CaptureDevice i_cap_device)
        {
            //キャプチャを作る(QVGAでフレームレートは15)
            i_cap_device.SetCaptureListener(this);
            i_cap_device.PrepareCapture(320, 240,15);
            this.m_cap = i_cap_device;
            
            //ARの設定

            //ARラスタを作る(DirectShowキャプチャ仕様)。
            this.m_raster = new DsXRGB32Raster(i_cap_device.video_width, i_cap_device.video_height, i_cap_device.video_width * i_cap_device.video_bit_count / 8);

            //AR用カメラパラメタファイルをロードして設定
            D3dARParam ap = new D3dARParam();
            ap.loadFromARFile(AR_CAMERA_FILE);
            ap.changeSize(320, 240);

            //AR用のパターンコードを読み出し	
            NyARCode code = new NyARCode(16, 16);
            code.loadFromARFile(AR_CODE_FILE);

            //１パターンのみを追跡するクラスを作成
            this.m_ar = new D3dSingleDetectMarker(ap, code, 80.0);

            //計算モードの設定
            this.m_ar.setContinueMode(false);


            //3dデバイスを準備する
            this._device = PrepareD3dDevice(topLevelForm);

            //カメラProjectionの設定
            this._device.Transform.Projection = ap.getCameraFrustumRH();

            // ビュー変換の設定(左手座標系ビュー行列で設定する)
            // 0,0,0から、Z+方向を向いて、上方向がY軸
            this._device.Transform.View = Matrix.LookAtLH(
                new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f), new Vector3(0.0f, 1.0f, 0.0f));

            //立方体（頂点数8）の準備
            this._vertexBuffer = new VertexBuffer(typeof(CustomVertex.PositionColored),
                8, this._device, Usage.None, CustomVertex.PositionColored.Format, Pool.Managed);

            //8点の情報を格納するためのメモリを確保
            CustomVertex.PositionColored[] vertices = new CustomVertex.PositionColored[8];
            const float CUBE_SIZE = 20.0f;//1辺40[mm]の
            //頂点を設定
            vertices[0] = new CustomVertex.PositionColored(-CUBE_SIZE, CUBE_SIZE, CUBE_SIZE, Color.Yellow.ToArgb());
            vertices[1] = new CustomVertex.PositionColored(CUBE_SIZE, CUBE_SIZE, CUBE_SIZE, Color.Gray.ToArgb());
            vertices[2] = new CustomVertex.PositionColored(-CUBE_SIZE, CUBE_SIZE, -CUBE_SIZE, Color.Purple.ToArgb());
            vertices[3] = new CustomVertex.PositionColored(CUBE_SIZE, CUBE_SIZE, -CUBE_SIZE, Color.Red.ToArgb());
            vertices[4] = new CustomVertex.PositionColored(-CUBE_SIZE, -CUBE_SIZE, CUBE_SIZE, Color.SkyBlue.ToArgb());
            vertices[5] = new CustomVertex.PositionColored(CUBE_SIZE, -CUBE_SIZE, CUBE_SIZE, Color.Orange.ToArgb());
            vertices[6] = new CustomVertex.PositionColored(-CUBE_SIZE, -CUBE_SIZE, -CUBE_SIZE, Color.Green.ToArgb());
            vertices[7] = new CustomVertex.PositionColored(CUBE_SIZE, -CUBE_SIZE, -CUBE_SIZE, Color.Blue.ToArgb());

            //頂点バッファをロックする
            using (GraphicsStream data = this._vertexBuffer.Lock(0, 0, LockFlags.None))
            {
                // 頂点データを頂点バッファにコピーします
                data.Write(vertices);

                // 頂点バッファのロックを解除します
                this._vertexBuffer.Unlock();
            }

            // インデックスバッファの作成
            // 第２引数の数値は(三角ポリゴンの数)*(ひとつの三角ポリゴンの頂点数)*
            // (16 ビットのインデックスサイズ(2byte))
            this._indexBuffer = new IndexBuffer(this._device, 12 * 3 * 2, Usage.WriteOnly,
                Pool.Managed, true);

            // インデックスバッファをロックする
            using (GraphicsStream data = this._indexBuffer.Lock(0, 0, LockFlags.None))
            {
                // インデックスデータをインデックスバッファにコピーします
                data.Write(_vertexIndices);

                // インデックスバッファのロックを解除します
                this._indexBuffer.Unlock();
            }
            // ライトを無効
            this._device.RenderState.Lighting = false;

            //背景用のスプライト
            this._sprite = new Sprite(this._device);

            // カリングを無効にしてポリゴンの裏も描画する
            //this._device.RenderState.CullMode = Cull.None;

            //背景テクスチャを作成
            this.m_texture = new NyARTexture_XRGB32(this._device, 320, 240);

            return true;
        }

        //メインループ処理
        public void MainLoop()
        {
            //ARの計算
            Matrix trans_matrix = new Matrix();
            bool is_marker_enable;
            lock (this)
            {
                //マーカーは見つかったかな？
                is_marker_enable = this.m_ar.detectMarkerLite(this.m_raster, 110);
                if (is_marker_enable)
                {
                    //あればMatrixを計算
                    this.m_ar.getD3dMatrix(out trans_matrix);
                }
            }
            // 描画内容を単色でクリア
            this._device.Clear(ClearFlags.Target, Color.DarkBlue, 1.0f, 0);

            // 3Dオブジェクトの描画はここから
            this._device.BeginScene();

            // 背景テクスチャスプライトで描画
            this._sprite.Begin(SpriteFlags.None);
            this._sprite.Draw(this.m_texture.d3d_texture, Rectangle.Empty, Vector3.Empty, Vector3.Empty, Color.White);
            this._sprite.End();


            //マーカーが見つかっていて、0.4より一致してたら描画する。
            if (is_marker_enable && this.m_ar.getConfidence()>0.4)
            {
                // 頂点バッファをデバイスのデータストリームにバインド
                this._device.SetStreamSource(0, this._vertexBuffer, 0);

                // 描画する頂点のフォーマットをセット
                this._device.VertexFormat = CustomVertex.PositionColored.Format;

                // インデックスバッファをセット
                this._device.Indices = this._indexBuffer;

                //立方体を20mm上（マーカーの上）にずらしておく
                Matrix transform_mat2 = Matrix.Translation(0,0,20.0f);

                //変換行列を掛ける
                transform_mat2 *= trans_matrix;
                // 計算したマトリックスで座標変換
                this._device.SetTransform(TransformType.World, transform_mat2);

                // レンダリング（描画）
                this._device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 8, 0, 12);
            }

            // 描画はここまで
            this._device.EndScene();

            // 実際のディスプレイに描画
            this._device.Present();
            
        }

        // リソースの破棄をするために呼ばれる
        public void Dispose()
        {
            // 頂点バッファを解放
            if (this._vertexBuffer != null)
            {
                this._vertexBuffer.Dispose();
            }

            // インデックスバッファを解放
            if (this._indexBuffer != null)
            {
                this._indexBuffer.Dispose();
            }              
            // Direct3D デバイスのリソース解放
            if (this._device != null)
            {
                this._device.Dispose();
            }
        }
    }
}