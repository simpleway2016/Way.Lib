using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OPC
{
    public class Helper
    {
        public static Type VT2TypeCode(VarEnum vevt)
        {
            switch (vevt)
            {
                case VarEnum.VT_I1:
                    return typeof(SByte);
                case VarEnum.VT_I2:
                    return typeof(Int16);
                case VarEnum.VT_I4:
                    return typeof(Int32);
                case VarEnum.VT_I8:
                    return typeof(Int64);

                case VarEnum.VT_UI1:
                    return typeof(byte);
                case VarEnum.VT_UI2:
                    return typeof(UInt16);
                case VarEnum.VT_UI4:
                    return typeof(UInt32);
                case VarEnum.VT_UI8:
                    return typeof(UInt64);

                case VarEnum.VT_R4:
                    return typeof(Single);
                case VarEnum.VT_R8:
                    return typeof(double);

                case VarEnum.VT_BSTR:
                    return typeof(string);
                case VarEnum.VT_BOOL:
                    return typeof(bool);
                case VarEnum.VT_DATE:
                    return typeof(DateTime);
                case VarEnum.VT_DECIMAL:
                    return typeof(decimal);
                case VarEnum.VT_CY:				// not supported
                    return typeof(double);
            }

            return null;
        }
    }
}
