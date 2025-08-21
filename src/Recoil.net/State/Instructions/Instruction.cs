using System;
using System.Collections.Generic;
using System.Text;

namespace RecoilNet.State.Instructions
{
    internal class Instruction
    {
        public Primitive Primitive { get; }

        public Instruction(Primitive primitive)
        {
            Gaurd.NotNull(primitive);
            Primitive = primitive;
        }
    }
}
