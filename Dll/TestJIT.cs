using System;
using System.Runtime.CompilerServices;

namespace Dll
{
    public class TestJIT
    {
        private static readonly bool CheckAccessible = true;
        protected virtual int C { get; }
        protected virtual int ReferenceCount => 1;

        public TestJIT(int c)
        {
            this.C = c;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void RunThrowWithCall(int a, int b)
        {
            //rep stos
            //IF IsOutOfBounds() JMP _2
            //retn
            //_2: throw
            CheckIndex0ThrowWithCall(a, b);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void X1()
        {
            X2();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void X2()
        {
            if (IsOutOfBounds(1, 1, this.C))
            {
                ThrowHelper.ThrowIndexOutOfRangeException("");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void RunThrowDirect(int a, int b)
        {
            //NO rep stos
            //IF IsOutOfBounds() JMP _2
            //retn
            //_2: call throw
            CheckIndex0ThrowDirect(a, b);
        }

        //[MethodImpl(MethodImplOptions.NoInlining)]
        public virtual int CheckIndexThrowWithCall(int index)
        {
            //rep stos
            //IF IsOutOfBounds() JMP _2
            //retn test()
            //_2: call throw
            CheckIndex0ThrowWithCall(index, 4);
            return Test(index);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public virtual int CheckIndexThrowWithCall2(int index)
        {
            //rep stos
            //IF IsOutOfBounds() JMP _2
            //retn test()
            //_2: call throw
            CheckIndex0ThrowWithCall(index, 4);
            return Test(index);
        }

        //[MethodImpl(MethodImplOptions.NoInlining)]
        public virtual int CheckIndexThrowDirect(int index)
        {
            //NO rep stos
            //IF IsOutOfBounds() JMP _2
            //jmp test
            //_2: call throw
            CheckIndex0ThrowDirect(index, 4);
            return Test(index);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public virtual int CheckIndexThrowDirect2(int index)
        {
            //NO rep stos
            //IF IsOutOfBounds() JMP _2
            //call test()
            //_2: call throw
            CheckIndex0ThrowDirect(index, 4);
            return Test(index);
        }

        protected virtual int Test(int a) => a;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void EnsureAccessible()
        {
            if (CheckAccessible && this.ReferenceCount == 0)
            {
                ThrowHelper.ThrowIllegalReferenceCountException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CheckIndex0ThrowWithCall(int index, int fieldLength)
        {
            //rep stos
            //IF !IsOutOfBounds() JMP _2
            //jmp throw
            //_2: retn
            if (IsOutOfBounds(index, fieldLength, this.C))
            {
                ThrowHelper.ThrowIndexOutOfRangeException($"index: {index}, length: {fieldLength} (expected: range(0, {this.C}))");//42,46
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CheckIndex0ThrowDirect(int index, int fieldLength)
        {
            //NO rep stos
            //IF IsOutOfBounds() JMP _2
            //retn
            //_2: call throw
            if (IsOutOfBounds(index, fieldLength, this.C))
            {
                throw ThrowHelper.CreateIndexOutOfRangeException($"index: {index}, length: {fieldLength} (expected: range(0, {this.C}))");//42,46
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IsOutOfBounds(int index, int length, int capacity) =>
            unchecked(index | length | (index + length) | (capacity - (index + length))) < 0;
    }

    internal class ThrowHelper
    {
        public static Exception CreateIndexOutOfRangeException(string message) => new IndexOutOfRangeException(message);
        public static void ThrowIndexOutOfRangeException(string message) => throw new IndexOutOfRangeException(message);
        public static void ThrowIllegalReferenceCountException(int count = 0) => throw new IllegalReferenceCountException(count);
    }


    public class IllegalReferenceCountException : InvalidOperationException
    {
        public IllegalReferenceCountException(int count)
            : base($"Illegal reference count of {count} for this object")
        {
        }

        public IllegalReferenceCountException(int refCnt, int increment)
            : base("refCnt: " + refCnt + ", " + (increment > 0 ? "increment: " + increment : "decrement: " + -increment))
        {
        }
    }
}
