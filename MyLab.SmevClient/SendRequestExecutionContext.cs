﻿using System;
using MyLab.SmevClient.Smev;

namespace MyLab.SmevClient
{
    public class SendRequestExecutionContext<T> where T: new()
    {
        /// <summary>
        /// Данные запроса
        /// </summary>
        public T RequestData { get; set; }

        /// <summary>
        /// Вложения
        /// </summary>
        public AttachmentContentList Attachments { get; set; }

        /// <summary>
        /// Флаг тестового запроса
        /// </summary>
        public bool IsTest { get; set; }
    }
}
