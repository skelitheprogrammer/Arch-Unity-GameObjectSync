﻿namespace Code.Arch.Arch.View
{
    public interface IViewHandler<T>
    {
        T Get();
        void Remove(T view);
    }
}