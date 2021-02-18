﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Smev3Client.Crypt;
using Smev3Client.Extensions;
using Smev3Client.Utils;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Smev3Client.Test
{
    class Program
    {
        static public async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                        .ConfigureServices((hostContext, services) =>
                        {
                            services.UseSmev3Client();
                        })
                        .ConfigureHostConfiguration(configHost => {

                            configHost.AddJsonFile("appsettings.json", optional: false);

                        })
                        .UseConsoleLifetime();

            var host = builder.Build();

            var factory = host.Services.GetRequiredService<ISmev3ClientFactory>();

            var client = factory.Get("SVC_MNEMONIC");

            var regCertData = new ESIARegisterCertificateRequestType
            {
                RoutingCode = EnvType.TESIA,
                serialNumber = "016FF958008DAC3F9042F0E907257D76F0",
                issuerOrgName = "ООО \"АСТРАЛ - М\"",
                startDate = "10.12.2020",
                expiryDate = "10.12.2020",
                ownerType = ownertypeType.IB,
                snils = "078-439-227 02",
                personINN = "166003726000",
                lastName = "Антонов",
                firstName = "Владимир",
                middleName = "Вячеславович",
                gender = genderType.M,
                genderSpecified = true,
                birthDate = "12.12.1970",
                birthPlace = "гор. Казань",
                citizenship = "RUS",
                ogrn = "319169000093278",
                doc = new document3Type()
            };
            regCertData.doc.type = documenttypeType.RF_PASSPORT;
            regCertData.doc.series = "9216";
            regCertData.doc.number = "013277";
            regCertData.doc.issueId = "160008";
            regCertData.doc.issueDate = "25.12.2015";

            using var response1 = await client.SendRequestAsync(regCertData, default);

            var str = await response1.HttpResponse.Content.ReadAsStringAsync();

            using var response2 = await client.GetResponseAsync(default);

            str = await response2.HttpResponse.Content.ReadAsStringAsync();

            using var response3 = await client.AckAsync(Guid.NewGuid(), default);

            str = await response3.HttpResponse.Content.ReadAsStringAsync();
        }
    }
}