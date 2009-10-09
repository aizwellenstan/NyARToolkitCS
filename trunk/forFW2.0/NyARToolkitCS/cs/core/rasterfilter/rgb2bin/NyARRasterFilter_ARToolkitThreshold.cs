/* 
 * PROJECT: NyARToolkitCS
 * --------------------------------------------------------------------------------
 * This work is based on the original ARToolKit developed by
 *   Hirokazu Kato
 *   Mark Billinghurst
 *   HITLab, University of Washington, Seattle
 * http://www.hitl.washington.edu/artoolkit/
 *
 * The NyARToolkit is Java version ARToolkit class library.
 * Copyright (C)2008 R.Iizuka
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this framework; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 * 
 * For further information please contact.
 *	http://nyatla.jp/nyatoolkit/
 *	<airmail(at)ebony.plala.or.jp>
 * 
 */
using System.Diagnostics;
namespace jp.nyatla.nyartoolkit.cs.core
{
    public class NyARRasterFilter_ARToolkitThreshold : INyARRasterFilter_RgbToBin
    {
        interface IdoThFilterImpl
        {
            void doThFilter(INyARBufferReader i_input, INyARBufferReader i_output, NyARIntSize i_size, int i_threshold);
        }
        class doThFilterImpl_BUFFERFORMAT_BYTE1D_RGB_24 : IdoThFilterImpl
        {
            public void doThFilter(INyARBufferReader i_input, INyARBufferReader i_output, NyARIntSize i_size, int i_threshold)
            {
                int[] out_buf = (int[])i_output.getBuffer();
                byte[] in_buf = (byte[])i_input.getBuffer();

                int th = i_threshold * 3;
                int bp = (i_size.w * i_size.h - 1) * 3;
                int w;
                int xy;
                int pix_count = i_size.h * i_size.w;
                int pix_mod_part = pix_count - (pix_count % 8);
                for (xy = pix_count - 1; xy >= pix_mod_part; xy--)
                {
                    w = ((in_buf[bp] & 0xff) + (in_buf[bp + 1] & 0xff) + (in_buf[bp + 2] & 0xff));
                    out_buf[xy] = w <= th ? 0 : 1;
                    bp -= 3;
                }
                //タイリング
                for (; xy >= 0; )
                {
                    w = ((in_buf[bp] & 0xff) + (in_buf[bp + 1] & 0xff) + (in_buf[bp + 2] & 0xff));
                    out_buf[xy] = w <= th ? 0 : 1;
                    bp -= 3;
                    xy--;
                    w = ((in_buf[bp] & 0xff) + (in_buf[bp + 1] & 0xff) + (in_buf[bp + 2] & 0xff));
                    out_buf[xy] = w <= th ? 0 : 1;
                    bp -= 3;
                    xy--;
                    w = ((in_buf[bp] & 0xff) + (in_buf[bp + 1] & 0xff) + (in_buf[bp + 2] & 0xff));
                    out_buf[xy] = w <= th ? 0 : 1;
                    bp -= 3;
                    xy--;
                    w = ((in_buf[bp] & 0xff) + (in_buf[bp + 1] & 0xff) + (in_buf[bp + 2] & 0xff));
                    out_buf[xy] = w <= th ? 0 : 1;
                    bp -= 3;
                    xy--;
                    w = ((in_buf[bp] & 0xff) + (in_buf[bp + 1] & 0xff) + (in_buf[bp + 2] & 0xff));
                    out_buf[xy] = w <= th ? 0 : 1;
                    bp -= 3;
                    xy--;
                    w = ((in_buf[bp] & 0xff) + (in_buf[bp + 1] & 0xff) + (in_buf[bp + 2] & 0xff));
                    out_buf[xy] = w <= th ? 0 : 1;
                    bp -= 3;
                    xy--;
                    w = ((in_buf[bp] & 0xff) + (in_buf[bp + 1] & 0xff) + (in_buf[bp + 2] & 0xff));
                    out_buf[xy] = w <= th ? 0 : 1;
                    bp -= 3;
                    xy--;
                    w = ((in_buf[bp] & 0xff) + (in_buf[bp + 1] & 0xff) + (in_buf[bp + 2] & 0xff));
                    out_buf[xy] = w <= th ? 0 : 1;
                    bp -= 3;
                    xy--;
                }
                return;
            }

        }
        class doThFilterImpl_BUFFERFORMAT_BYTE1D_B8G8R8X8_32 : IdoThFilterImpl
        {
            public void doThFilter(INyARBufferReader i_input, INyARBufferReader i_output, NyARIntSize i_size, int i_threshold)
            {
                int[] out_buf = (int[])i_output.getBuffer();
                byte[] in_buf = (byte[])i_input.getBuffer();

                int th = i_threshold * 3;
                int bp = (i_size.w * i_size.h - 1) * 4;
                int w;
                int xy;
                int pix_count = i_size.h * i_size.w;
                int pix_mod_part = pix_count - (pix_count % 8);
                for (xy = pix_count - 1; xy >= pix_mod_part; xy--)
                {
                    w = ((in_buf[bp] & 0xff) + (in_buf[bp + 1] & 0xff) + (in_buf[bp + 2] & 0xff));
                    out_buf[xy] = w <= th ? 0 : 1;
                    bp -= 4;
                }
                //タイリング
                for (; xy >= 0; )
                {
                    w = ((in_buf[bp] & 0xff) + (in_buf[bp + 1] & 0xff) + (in_buf[bp + 2] & 0xff));
                    out_buf[xy] = w <= th ? 0 : 1;
                    bp -= 4;
                    xy--;
                    w = ((in_buf[bp] & 0xff) + (in_buf[bp + 1] & 0xff) + (in_buf[bp + 2] & 0xff));
                    out_buf[xy] = w <= th ? 0 : 1;
                    bp -= 4;
                    xy--;
                    w = ((in_buf[bp] & 0xff) + (in_buf[bp + 1] & 0xff) + (in_buf[bp + 2] & 0xff));
                    out_buf[xy] = w <= th ? 0 : 1;
                    bp -= 4;
                    xy--;
                    w = ((in_buf[bp] & 0xff) + (in_buf[bp + 1] & 0xff) + (in_buf[bp + 2] & 0xff));
                    out_buf[xy] = w <= th ? 0 : 1;
                    bp -= 4;
                    xy--;
                    w = ((in_buf[bp] & 0xff) + (in_buf[bp + 1] & 0xff) + (in_buf[bp + 2] & 0xff));
                    out_buf[xy] = w <= th ? 0 : 1;
                    bp -= 4;
                    xy--;
                    w = ((in_buf[bp] & 0xff) + (in_buf[bp + 1] & 0xff) + (in_buf[bp + 2] & 0xff));
                    out_buf[xy] = w <= th ? 0 : 1;
                    bp -= 4;
                    xy--;
                    w = ((in_buf[bp] & 0xff) + (in_buf[bp + 1] & 0xff) + (in_buf[bp + 2] & 0xff));
                    out_buf[xy] = w <= th ? 0 : 1;
                    bp -= 4;
                    xy--;
                    w = ((in_buf[bp] & 0xff) + (in_buf[bp + 1] & 0xff) + (in_buf[bp + 2] & 0xff));
                    out_buf[xy] = w <= th ? 0 : 1;
                    bp -= 4;
                    xy--;
                }
            }
        }

        class doThFilterImpl_BUFFERFORMAT_BYTE1D_X8R8G8B8_32 : IdoThFilterImpl
        {
            public void doThFilter(INyARBufferReader i_input, INyARBufferReader i_output, NyARIntSize i_size, int i_threshold)
            {
                int[] out_buf = (int[])i_output.getBuffer();
                byte[] in_buf = (byte[])i_input.getBuffer();

                int th = i_threshold * 3;
                int bp = (i_size.w * i_size.h - 1) * 4;
                int w;
                int xy;
                int pix_count = i_size.h * i_size.w;
                int pix_mod_part = pix_count - (pix_count % 8);
                for (xy = pix_count - 1; xy >= pix_mod_part; xy--)
                {
                    w = ((in_buf[bp + 1] & 0xff) + (in_buf[bp + 2] & 0xff) + (in_buf[bp + 3] & 0xff));
                    out_buf[xy] = w <= th ? 0 : 1;
                    bp -= 4;
                }
                //タイリング
                for (; xy >= 0; )
                {
                    w = ((in_buf[bp + 1] & 0xff) + (in_buf[bp + 2] & 0xff) + (in_buf[bp + 3] & 0xff));
                    out_buf[xy] = w <= th ? 0 : 1;
                    bp -= 4;
                    xy--;
                    w = ((in_buf[bp + 1] & 0xff) + (in_buf[bp + 2] & 0xff) + (in_buf[bp + 3] & 0xff));
                    out_buf[xy] = w <= th ? 0 : 1;
                    bp -= 4;
                    xy--;
                    w = ((in_buf[bp + 1] & 0xff) + (in_buf[bp + 2] & 0xff) + (in_buf[bp + 3] & 0xff));
                    out_buf[xy] = w <= th ? 0 : 1;
                    bp -= 4;
                    xy--;
                    w = ((in_buf[bp + 1] & 0xff) + (in_buf[bp + 2] & 0xff) + (in_buf[bp + 3] & 0xff));
                    out_buf[xy] = w <= th ? 0 : 1;
                    bp -= 4;
                    xy--;
                    w = ((in_buf[bp + 1] & 0xff) + (in_buf[bp + 2] & 0xff) + (in_buf[bp + 3] & 0xff));
                    out_buf[xy] = w <= th ? 0 : 1;
                    bp -= 4;
                    xy--;
                    w = ((in_buf[bp + 1] & 0xff) + (in_buf[bp + 2] & 0xff) + (in_buf[bp + 3] & 0xff));
                    out_buf[xy] = w <= th ? 0 : 1;
                    bp -= 4;
                    xy--;
                    w = ((in_buf[bp + 1] & 0xff) + (in_buf[bp + 2] & 0xff) + (in_buf[bp + 3] & 0xff));
                    out_buf[xy] = w <= th ? 0 : 1;
                    bp -= 4;
                    xy--;
                    w = ((in_buf[bp + 1] & 0xff) + (in_buf[bp + 2] & 0xff) + (in_buf[bp + 3] & 0xff));
                    out_buf[xy] = w <= th ? 0 : 1;
                    bp -= 4;
                    xy--;
                }
                return;
            }

        }

        class doThFilterImpl_BUFFERFORMAT_INT1D_X8R8G8B8_32 : IdoThFilterImpl
        {
            public void doThFilter(INyARBufferReader i_input, INyARBufferReader i_output, NyARIntSize i_size, int i_threshold)
            {
                int[] out_buf = (int[])i_output.getBuffer();
                int[] in_buf = (int[])i_input.getBuffer();

                int th = i_threshold * 3;
                int w;
                int xy;
                int pix_count = i_size.h * i_size.w;
                int pix_mod_part = pix_count - (pix_count % 8);

                for (xy = pix_count - 1; xy >= pix_mod_part; xy--)
                {
                    w = in_buf[xy];
                    out_buf[xy] = (((w >> 16) & 0xff) + ((w >> 8) & 0xff) + (w & 0xff)) <= th ? 0 : 1;
                }
                //タイリング
                for (; xy >= 0; )
                {
                    w = in_buf[xy];
                    out_buf[xy] = (((w >> 16) & 0xff) + ((w >> 8) & 0xff) + (w & 0xff)) <= th ? 0 : 1;
                    xy--;
                    w = in_buf[xy];
                    out_buf[xy] = (((w >> 16) & 0xff) + ((w >> 8) & 0xff) + (w & 0xff)) <= th ? 0 : 1;
                    xy--;
                    w = in_buf[xy];
                    out_buf[xy] = (((w >> 16) & 0xff) + ((w >> 8) & 0xff) + (w & 0xff)) <= th ? 0 : 1;
                    xy--;
                    w = in_buf[xy];
                    out_buf[xy] = (((w >> 16) & 0xff) + ((w >> 8) & 0xff) + (w & 0xff)) <= th ? 0 : 1;
                    xy--;
                    w = in_buf[xy];
                    out_buf[xy] = (((w >> 16) & 0xff) + ((w >> 8) & 0xff) + (w & 0xff)) <= th ? 0 : 1;
                    xy--;
                    w = in_buf[xy];
                    out_buf[xy] = (((w >> 16) & 0xff) + ((w >> 8) & 0xff) + (w & 0xff)) <= th ? 0 : 1;
                    xy--;
                    w = in_buf[xy];
                    out_buf[xy] = (((w >> 16) & 0xff) + ((w >> 8) & 0xff) + (w & 0xff)) <= th ? 0 : 1;
                    xy--;
                    w = in_buf[xy];
                    out_buf[xy] = (((w >> 16) & 0xff) + ((w >> 8) & 0xff) + (w & 0xff)) <= th ? 0 : 1;
                    xy--;
                }
            }
        }

        class doThFilterImpl_BUFFERFORMAT_WORD1D_R5G6B5_16LE : IdoThFilterImpl
        {
            public void doThFilter(INyARBufferReader i_input, INyARBufferReader i_output, NyARIntSize i_size, int i_threshold)
            {
                int[] out_buf = (int[])i_output.getBuffer();
                short[] in_buf = (short[])i_input.getBuffer();

                int th = i_threshold * 3;
                int w;
                int xy;
                int pix_count = i_size.h * i_size.w;
                int pix_mod_part = pix_count - (pix_count % 8);

                for (xy = pix_count - 1; xy >= pix_mod_part; xy--)
                {
                    w = (int)in_buf[xy];
                    w = ((w & 0xf800) >> 8) + ((w & 0x07e0) >> 3) + ((w & 0x001f) << 3);
                    out_buf[xy] = w <= th ? 0 : 1;
                }
                //タイリング
                for (; xy >= 0; )
                {
                    w = (int)in_buf[xy];
                    w = ((w & 0xf800) >> 8) + ((w & 0x07e0) >> 3) + ((w & 0x001f) << 3);
                    out_buf[xy] = w <= th ? 0 : 1;
                    xy--;
                    w = (int)in_buf[xy];
                    w = ((w & 0xf800) >> 8) + ((w & 0x07e0) >> 3) + ((w & 0x001f) << 3);
                    out_buf[xy] = w <= th ? 0 : 1;
                    xy--;
                    w = (int)in_buf[xy];
                    w = ((w & 0xf800) >> 8) + ((w & 0x07e0) >> 3) + ((w & 0x001f) << 3);
                    out_buf[xy] = w <= th ? 0 : 1;
                    xy--;
                    w = (int)in_buf[xy];
                    w = ((w & 0xf800) >> 8) + ((w & 0x07e0) >> 3) + ((w & 0x001f) << 3);
                    out_buf[xy] = w <= th ? 0 : 1;
                    xy--;
                    w = (int)in_buf[xy];
                    w = ((w & 0xf800) >> 8) + ((w & 0x07e0) >> 3) + ((w & 0x001f) << 3);
                    out_buf[xy] = w <= th ? 0 : 1;
                    xy--;
                    w = (int)in_buf[xy];
                    w = ((w & 0xf800) >> 8) + ((w & 0x07e0) >> 3) + ((w & 0x001f) << 3);
                    out_buf[xy] = w <= th ? 0 : 1;
                    xy--;
                    w = (int)in_buf[xy];
                    w = ((w & 0xf800) >> 8) + ((w & 0x07e0) >> 3) + ((w & 0x001f) << 3);
                    out_buf[xy] = w <= th ? 0 : 1;
                    xy--;
                    w = (int)in_buf[xy];
                    w = ((w & 0xf800) >> 8) + ((w & 0x07e0) >> 3) + ((w & 0x001f) << 3);
                    out_buf[xy] = w <= th ? 0 : 1;
                    xy--;
                }
            }
        }

        private int _threshold;
        private IdoThFilterImpl _do_threshold_impl;

        public NyARRasterFilter_ARToolkitThreshold(int i_threshold, int i_input_raster_type)
        {
            this._threshold = i_threshold;
            switch (i_input_raster_type)
            {
                case INyARBufferReader.BUFFERFORMAT_BYTE1D_B8G8R8_24:
                case INyARBufferReader.BUFFERFORMAT_BYTE1D_R8G8B8_24:
                    this._do_threshold_impl = new doThFilterImpl_BUFFERFORMAT_BYTE1D_RGB_24();
                    break;
                case INyARBufferReader.BUFFERFORMAT_BYTE1D_B8G8R8X8_32:
                    this._do_threshold_impl = new doThFilterImpl_BUFFERFORMAT_BYTE1D_B8G8R8X8_32();
                    break;
                case INyARBufferReader.BUFFERFORMAT_BYTE1D_X8R8G8B8_32:
                    this._do_threshold_impl = new doThFilterImpl_BUFFERFORMAT_BYTE1D_X8R8G8B8_32();
                    break;
                case INyARBufferReader.BUFFERFORMAT_INT1D_X8R8G8B8_32:
                    this._do_threshold_impl = new doThFilterImpl_BUFFERFORMAT_INT1D_X8R8G8B8_32();
                    break;
                case INyARBufferReader.BUFFERFORMAT_WORD1D_R5G6B5_16LE:
                    this._do_threshold_impl = new doThFilterImpl_BUFFERFORMAT_WORD1D_R5G6B5_16LE();
                    break;

                default:
                    throw new NyARException();
            }


        }
        public void setThreshold(int i_threshold)
        {
            this._threshold = i_threshold;
        }

        public void doFilter(INyARRgbRaster i_input, NyARBinRaster i_output)
        {
            INyARBufferReader in_buffer_reader = i_input.getBufferReader();
            INyARBufferReader out_buffer_reader = i_output.getBufferReader();

            Debug.Assert(out_buffer_reader.isEqualBufferType(INyARBufferReader.BUFFERFORMAT_INT1D_BIN_8));
            Debug.Assert(i_input.getSize().isEqualSize(i_output.getSize()) == true);
            this._do_threshold_impl.doThFilter(in_buffer_reader, out_buffer_reader, i_output.getSize(), this._threshold);
            return;
        }

    }
}