﻿using System.Windows;

namespace VrPlayer.Contracts
{
    public interface IPlugin<T>
    {
        string Name { get; set; }
        T Content { get; set; }
        FrameworkElement Panel { get; set; }
    }
}