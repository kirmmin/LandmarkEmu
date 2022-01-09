﻿using System;

namespace LandmarkEmulator.Shared
{
    public abstract class Singleton<T> where T : class
    {
        public static T Instance => instance.Value;

        private static readonly Lazy<T> instance = new(() =>
            Activator.CreateInstance(typeof(T), true) as T);
    }
}
