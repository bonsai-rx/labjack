using LabJack;
using OpenCV.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bonsai.LabJack
{
    public class AnalogInput : Source<Mat>
    {
        public AnalogInput()
        {
            ScanNames = new[] { "AIN0" };
            ScansPerRead = 1000;
        }

        public string[] ScanNames { get; set; }

        public int ScansPerRead { get; set; }

        static void ThrowExceptionForErrorCode(LJM.LJMERROR error)
        {
            if (error != LJM.LJMERROR.NOERROR)
            {
                throw new LJM.LJMException(error);
            }
        }

        public override IObservable<Mat> Generate()
        {
            return Observable.Create<Mat>((observer, cancellationToken) =>
            {
                return Task.Factory.StartNew(() =>
                {
                    var handle = 0;
                    try
                    {
                        var error = LJM.Open((int)DeviceType.Any, (int)ConnectionType.Any, "ANY", ref handle);
                        ThrowExceptionForErrorCode(error);

                        var scanNames = ScanNames;
                        if (scanNames == null || scanNames.Length == 0)
                        {
                            throw new InvalidOperationException("At least one input channel name must be specified.");
                        }

                        var scansPerRead = ScansPerRead;
                        var addresses = new int[scanNames.Length];
                        var addressTypes = new int[scanNames.Length];
                        error = LJM.NamesToAddresses(addresses.Length, scanNames, addresses, addressTypes);
                        ThrowExceptionForErrorCode(error);

                        double scanRate = scansPerRead;
                        error = LJM.eStreamStart(handle, scansPerRead, addresses.Length, addresses, ref scanRate);
                        ThrowExceptionForErrorCode(error);

                        var ljmScanBacklog = 0;
                        var deviceScanBacklog = 0;
                        var data = new double[scansPerRead * addresses.Length];
                        while (!cancellationToken.IsCancellationRequested)
                        {
                            LJM.eStreamRead(handle, data, ref deviceScanBacklog, ref ljmScanBacklog);
                            var output = Mat.FromArray(data, addresses.Length, scansPerRead, Depth.F64, 1);
                            observer.OnNext(output);
                        }

                        LJM.eStreamStop(handle);
                    }
                    catch(Exception ex) { observer.OnError(ex); }
                    finally
                    {
                        LJM.Close(handle);
                    }
                },
                cancellationToken,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
            });
        }
    }
}
