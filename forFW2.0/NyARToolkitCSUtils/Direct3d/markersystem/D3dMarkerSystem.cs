﻿using System;
using jp.nyatla.nyartoolkit.cs.markersystem;
#if NyartoolkitCS_FRAMEWORK_CFW
using Microsoft.WindowsMobile.DirectX.Direct3D;
using Microsoft.WindowsMobile.DirectX;
#else
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
#endif
namespace NyARToolkitCSUtils.Direct3d
{
    class D3dMarkerSystem : NyARMarkerSystem
    {
        public D3dMarkerSystem(INyARMarkerSystemConfig i_config)
            : base(i_config)
        {
        }
        protected void initInstance(INyARMarkerSystemConfig i_config)
        {
            base.initInstance(i_config);
        }

        private readonly Matrix _projection_mat = new Matrix();

        /**
         * OpenGLスタイルのProjectionMatrixを返します。
         * @param i_gl
         * @return
         * [readonly]
         */
        public Matrix getGlProjectionMatrix()
        {
            return this._projection_mat;
        }

        public void setProjectionMatrixClipping(double i_near, double i_far)
        {
            base.setProjectionMatrixClipping(i_near, i_far);
            NyARD3dUtil.toCameraFrustumRH(this._ref_param, i_near, i_far, this._projection_mat);
        }
        private readonly Matrix _work = new Matrix();

        /**
         * 
         * この関数はDirect3d形式の姿勢変換行列を返します。
         * 返却値の有効期間は、次回の{@link #getGlMarkerTransMat()}をコールするまでです。
         * 値を保持する場合は、{@link #getGlMarkerMatrix(double[])}を使用します。
         * @param i_buf
         * @return
         * [readonly]
         */
        public Matrix getD3dMarkerMatrix(int i_id)
        {
            return this.getD3dMarkerMatrix(i_id, this._work);
        }
        /**
         * この関数は、i_bufに指定idのOpenGL形式の姿勢変換行列を設定して返します。
         * @param i_id
         * @param i_buf
         * @return
         */
        public Matrix getD3dMarkerMatrix(int i_id, ref Matrix i_buf)
        {
            NyARD3dUtil.toD3dCameraView(this.getMarkerMatrix(i_id), 1, i_buf);
            return i_buf;
        }
    }
}
