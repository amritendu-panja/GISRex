﻿namespace Common.Exceptions
{
    public class DbException : Exception
    {
        public DbException(string? message): base(message)
        {
        }
    }
}
