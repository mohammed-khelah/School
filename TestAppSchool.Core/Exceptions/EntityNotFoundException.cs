﻿namespace TestAppSchool.Core.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() : base("العنصر غير موجود")
        {
        }
    }
}
