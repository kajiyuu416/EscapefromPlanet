using System.Collections.ObjectModel;
using System;

//定数クラス、他クラスからの参照
namespace Const
{
    public static class CO
    {
        public static readonly ReadOnlyCollection<float> Const_Float_List =
                    Array.AsReadOnly(new float[] { 1.0f, 2.0f, 3.0f,4.0f,5,0f});
        public static readonly ReadOnlyCollection<int> Const_Int_List =
            Array.AsReadOnly(new int[] { 1, 2, 3, 4, 5});

    }

}