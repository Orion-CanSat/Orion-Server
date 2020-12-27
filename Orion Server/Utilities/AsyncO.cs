#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrionServer.Utilities
{
    public class AsyncO
    {
        protected Stream? _stream = null;
        protected List<byte> _bytes = new();

        public AsyncO(Stream stream) => _stream = stream;
        public AsyncO(StreamWriter stream) => _stream = stream.BaseStream;

        public async Task<AsyncO> Write(byte[] bytes, int offset = 0, int length = -1)
        {
            if (_stream == null)
                throw new IOException();
            await _stream.WriteAsync(bytes, offset, (length != -1) ? length : bytes.Length);
            return this;
        }
        public async Task<AsyncO> Write(char[] chars, int offset = 0, int length = -1)
        {
            return await Write(Encoding.GetEncoding("UTF-8").GetBytes(chars), offset, length);
        }
        public async Task<AsyncO> Write(string text)
        {
            return await Write(text.ToArray());
        }

        public async Task<AsyncO> WriteLine(byte[] bytes, int offset = 0, int length = -1)
        {
            if (_stream == null)
                throw new IOException();
            await _stream.WriteAsync(bytes, offset, (length != -1) ? length : bytes.Length);
            await _stream.WriteAsync(new byte[] { (byte)'\n' }, 0, 1);
            return this;
        }
        public async Task<AsyncO> WriteLine(char[] chars, int offset = 0, int length = -1)
        {
            return await WriteLine(Encoding.GetEncoding("UTF-8").GetBytes(chars), offset, length);
        }
        public async Task<AsyncO> WriteLine(string text)
        {
            return await WriteLine(text.ToArray());
        }

        ~AsyncO() => _stream = null;
    }
}
