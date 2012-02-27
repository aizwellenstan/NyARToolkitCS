using System;
using jp.nyatla.nyartoolkit.cs.core;
using UnityEngine;
namespace NyARUnityUtils
{
	public static class NyARUnityUtil
	{
		/**
		 * NyARToolKit 2.53以前のコードと互換性を持たせるためのスケール値。
		 * {@link #toCameraFrustumRH}のi_scaleに設定することで、以前のバージョンの数値系と互換性を保ちます。
		 */
		public const double SCALE_FACTOR_toCameraFrustumRH_NYAR2=1.0;
		/**
		 * NyARToolKit 2.53以前のコードと互換性を持たせるためのスケール値。
		 * {@link #toCameraViewRH}のi_scaleに設定することで、以前のバージョンの数値系と互換性を保ちます。
		 */
		public const double SCALE_FACTOR_toCameraViewRH_NYAR2=1/0.025;
	
	
		/**
		 * この関数は、ARToolKitスタイルのカメラパラメータから、 CameraFrustamを計算します。
		 * カメラパラメータの要素のうち、ProjectionMatrix成分のみを使います。
		 * @param i_arparam
		 * ARToolKitスタイルのカメラパラメータ。
		 * @param i_scale
		 * スケール値を指定します。1=1mmです。10ならば1=1cm,1000ならば1=1mです。
		 * 2.53以前のNyARToolkitと互換性を持たせるときは、{@link #SCALE_FACTOR_toCameraFrustumRH_NYAR2}を指定してください。
		 * @param i_near
		 * 視錐体のnearPointを指定します。単位は、i_scaleに設定した値で決まります。
		 * @param i_far
		 * 視錐体のfarPointを指定します。単位は、i_scaleに設定した値で決まります。
		 * @param o_gl_projection
		 * OpenGLスタイルのProjectionMatrixです。double[16]を指定します。
		 */
		public static void toCameraFrustumRH(NyARParam i_arparam,double i_scale,double i_near,double i_far,ref Matrix4x4 o_mat)
		{
			toCameraFrustumRH(i_arparam.getPerspectiveProjectionMatrix(),i_arparam.getScreenSize(),i_scale,i_near,i_far,ref o_mat);
			return;
		}
		/**
		 * この関数は、ARToolKitスタイルのProjectionMatrixから、 CameraFrustamを計算します。
		 * @param i_promat
		 * @param i_size
		 * スクリーンサイズを指定します。
		 * @param i_scale
		 * {@link #toCameraFrustumRH(NyARParam i_arparam,double i_scale,double i_near,double i_far,double[] o_gl_projection)}を参照。
		 * @param i_near
		 * {@link #toCameraFrustumRH(NyARParam i_arparam,double i_scale,double i_near,double i_far,double[] o_gl_projection)}を参照。
		 * @param i_far
		 * {@link #toCameraFrustumRH(NyARParam i_arparam,double i_scale,double i_near,double i_far,double[] o_gl_projection)}を参照。
		 * @param o_gl_projection
		 * {@link #toCameraFrustumRH(NyARParam i_arparam,double i_scale,double i_near,double i_far,double[] o_gl_projection)}を参照。
		 */
		public static void toCameraFrustumRH(NyARPerspectiveProjectionMatrix i_promat,NyARIntSize i_size,double i_scale,double i_near,double i_far,ref Matrix4x4 o_mat)
		{
			NyARDoubleMatrix44 m=new NyARDoubleMatrix44();
			i_promat.makeCameraFrustumRH(i_size.w,i_size.h,i_near*i_scale,i_far*i_scale,m);
			o_mat.m00=(float)m.m00;
			o_mat.m01=(float)m.m01;
			o_mat.m02=(float)m.m02;
			o_mat.m03=(float)m.m03;
			o_mat.m10=(float)m.m10;
			o_mat.m11=(float)m.m11;
			o_mat.m12=(float)m.m12;
			o_mat.m13=(float)m.m13;
			o_mat.m20=(float)m.m20;
			o_mat.m21=(float)m.m21;
			o_mat.m22=(float)m.m22;
			o_mat.m23=(float)m.m23;
			o_mat.m30=(float)m.m30;
			o_mat.m31=(float)m.m31;
			o_mat.m32=(float)m.m32;
			o_mat.m33=(float)m.m33;
			return;
		}
		/**
		 * この関数は、NyARTransMatResultをOpenGLのModelView行列へ変換します。
		 * @param mat
		 * 変換元の行列
		 * @param i_scale
		 * 座標系のスケール値を指定します。1=1mmです。10ならば1=1cm,1000ならば1=1mです。
		 * 2.53以前のNyARToolkitと互換性を持たせるときは、{@link #SCALE_FACTOR_toCameraViewRH_NYAR2}を指定してください。
		 * @param o_gl_result
		 * OpenGLスタイルのProjectionMatrixです。double[16]を指定します。
		 */
		public static void toCameraViewRH(NyARDoubleMatrix44 mat,double i_scale,ref Matrix4x4 o_mat)
		{
			o_mat.m00 =-(float)mat.m00; 
			o_mat.m10 =(float)mat.m10;
			o_mat.m20 =-(float)mat.m20;
			o_mat.m30 =  (float)0.0;
			
			o_mat.m01  =-(float)mat.m01;
			o_mat.m11  =(float)mat.m11;
			o_mat.m21  =-(float)mat.m21;
			o_mat.m31  =(float)0.0;
			
			o_mat.m02  =-(float)mat.m02;
			o_mat.m12  =(float)mat.m12;
			o_mat.m22  =-(float)mat.m22;
			o_mat.m32  = (float)0.0;		
			double scale=1/i_scale;
			o_mat.m03 =(float)(mat.m03*scale);
			o_mat.m13 =-(float)(mat.m13*scale);
			o_mat.m23 =(float)(mat.m23*scale);
			o_mat.m33 = (float)1.0;
			return;
		}

		
	}
}

