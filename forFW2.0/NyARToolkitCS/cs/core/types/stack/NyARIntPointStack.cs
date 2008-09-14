/* 
 * PROJECT: NyARToolkit
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
using jp.nyatla.nyartoolkit.cs.utils;

namespace jp.nyatla.nyartoolkit.cs.core.types.stack
{

    public class NyARIntPointStack : NyObjectStack
    {
        public NyARIntPointStack(int i_length)
        {
            base(new NyARIntPoint[i_length]);

        }

        protected void onReservRequest(int i_start, int i_end, Object[] i_buffer)
        {
            for (int i = i_start; i < i_end; i++)
            {
                i_buffer[i] = new NyARIntPoint();
            }
        }

        public NyARIntPoint[] getArray()
        {
            return (NyARIntPoint[])this._items;
        }

        public NyARIntPoint prePush()
        {
            return (NyARIntPoint)super.prePush();
        }
    }
}