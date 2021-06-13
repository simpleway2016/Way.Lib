using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.AutoRunning
{
    enum MgrStatus
    {
        Stopped = 0,
        Running = 1,
        ReadyToStop = 2,
        
    }
    /// <summary>
    /// 记录自定运行管理类的状态
    /// </summary>
    class AutoRunMgrStatus
    {
        MemoryMappedFile mFile;
        MemoryMappedViewAccessor mAccessor;
        public AutoRunMgrStatus()
        {
            //可能多个站点使用同一个iis应用池，那就是同一个进程，所以共享内存的名称以目录作为区分
            string memoryName = AppDomain.CurrentDomain.BaseDirectory;
            memoryName = memoryName.Replace(":","").Replace("\\", "_").ToLower();
            mFile = MemoryMappedFile.CreateOrOpen("Global/" + memoryName, 10, MemoryMappedFileAccess.ReadWrite);
            mAccessor = mFile.CreateViewAccessor(0, 100);
            this.CurrentGuid = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 记录当前自动运行所属的唯一ID
        /// </summary>
        public string CurrentGuid
        {
            get
            {

                //读出字符串的长度
                int len = mAccessor.ReadInt32(4);
                //根据长度获取字符串
                byte[] buffer = new byte[len];
                for (int i = 0; i < len; i++)
                {
                    buffer[i] = mAccessor.ReadByte(8+i);
                }
                return System.Text.Encoding.UTF8.GetString(buffer);
            }
            set
            {
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(value);
                int len = buffer.Length;
                //先写入字符串的长度
                mAccessor.Write(4, len);
                //写入字符串
                for (int i = 0; i < len; i++)
                {
                    mAccessor.Write(8 + i, buffer[i]);
                }
            }
        }

        public MgrStatus Status
        {
            get
            {
                int status = mAccessor.ReadInt32(0);
                return (MgrStatus)status;
            }
            set
            {
                mAccessor.Write(0, (int)value);
            }
        }
    }
}
