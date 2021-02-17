﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Smev3Client
{
    internal class Smev3Client :
        IDisposable, ISmev3Client
    {
        #region members

        /// <summary>
        /// Параметры клиента
        /// </summary>
        private readonly ISmev3ClientContext _context;        

        #endregion        

        public Smev3Client(ISmev3ClientContext context)
        {
            _context = context ?? 
                throw new ArgumentNullException(nameof(context));            
        }

        /// <summary>
        /// Отправка конверта
        /// </summary>
        /// <param name="requestData"></param>
        public async Task<Smev3ClientResponse> SendAsync(
            ISmev3Envelope envelope,
            CancellationToken cancellationToken)
        {
            if (envelope == null)
                throw new ArgumentNullException(nameof(envelope));

            var envelopeBytes = envelope.Get();

            var encoding = new UTF8Encoding(true);

            var str = encoding.GetString(envelopeBytes);

            var content = new ByteArrayContent(
                envelopeBytes, 0, envelopeBytes.Length);

            content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Text.Xml)
            {
                CharSet = "utf-8"
            };

            cancellationToken.ThrowIfCancellationRequested();

            using var httpClient = _context.HttpClientFactory.CreateClient("smev");

            var response = await httpClient.PostAsync(
                string.Empty,
                content,
                cancellationToken);

            return new Smev3ClientResponse
            {
                HttpResponse = response
            };
        }

        #region IDisposable

        public void Dispose()
        {            
        }

        #endregion        
    }
}
