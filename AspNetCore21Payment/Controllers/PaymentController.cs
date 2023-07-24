using AspNetCore21Payment.Models;
using AspNetCore21Payment.Models.Melli;
using AspNetCore21Payment.Models.Pay;
using AspNetCore21Payment.Models.PaymentResult;
using AspNetCore21Payment.Models.Zarinpal;
using BankParsian;
using BankParsianConfirmPayment;
using BankParsianReversal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore21Payment.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IEpaymentRepository _epaymentRepository;
        public PaymentController(IEpaymentRepository epaymentRepository)
        {
            this._epaymentRepository = epaymentRepository;
        }

        #region اکشن اتصال به درگاه
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(long price, string dargahpardakht)
        {
            try
            {
                // آدرس برگشت از درگاه
                string redirectPage = "http://localhost:1980/Payment/";

                // ثبت اطلاعات اولیه پرداخت در دیتابیس
                int paymentId = InsertPayment(price, dargahpardakht);

                if (paymentId > 0)
                {
                    switch (dargahpardakht)
                    {
                        case "ملت":
                            redirectPage += "MellatCallBack";
                            await MellatPayment(price, redirectPage, paymentId);
                            break;
                        case "سامان":
                            redirectPage += "SamanCallBack";
                            // فراخواتی متد پرداخت
                            await SamanPayment(price, redirectPage, paymentId);
                            break;
                        case "ملی":
                            redirectPage += "MelliCallBack";
                            // فراخواتی متد پرداخت
                            await MelliPayment(price, redirectPage, paymentId);
                            break;
                        case "پارسیان":
                            redirectPage += "ParsianCallBack";
                            // فراخواتی متد پرداخت
                            await ParsianPayment(price, redirectPage, paymentId);
                            break;
                        case "پی":
                            redirectPage += "PayCallBack";
                            // فراخواتی متد پرداخت
                            await PayIrPayment(price, redirectPage, paymentId);
                            break;
                        case "زرین پال":
                            redirectPage += "ZarinpalCallBack";
                            redirectPage += "?OrderId=" + paymentId.ToString();
                            // فراخواتی متد پرداخت
                            await ZarinpalPayment((int)price, redirectPage, paymentId);
                            break;
                        default:
                            ViewData["Message"] = "درگاهی برای پرداخت انتخاب نشده است";
                            break;
                    }
                }
                else
                {
                    ViewData["Message"] = "پاسخی از درگاه دریافت نشد";
                }
            }
            catch
            {
                ViewData["Message"] = "پاسخی از درگاه دریافت نشد";

            }
            return View();
        }
        #endregion

        #region اکشن برگشت از درگاه
        public ActionResult Return(PurchaseResult result)
        {
            return View();
        }
        #endregion


        #region اکشن برگشت از درگاه بانک ملی
        public async Task<ActionResult> MelliCallBack(PurchaseResult result)
        {
            ViewData["BankName"] = "درگاه بانک ملی";

            // فراخوانی متد برگشت از درگاه
            await MelliReturn(result);

            return View("Return");
        }
        #endregion

        #region اکشن برگشت از درگاه زرین پال
        public async Task<ActionResult> ZarinpalCallBack(ZarinpalPurchaseResult result)
        {
            ViewData["BankName"] = "درگاه زرین پال";

            // فراخوانی متد برگشت از درگاه
            await ZarinpalReturn(result);

            return View("Return");
        }
        #endregion

        #region اکشن برگشت از درگاه پی
        public async Task<ActionResult> PayCallBack(PayPurchaseResult result)
        {
            ViewData["BankName"] = "درگاه پی";

            // فراخوانی متد برگشت از درگاه
            await PayIrReturn(result);

            return View("Return");
        }
        #endregion

        #region اکشن برگشت از درگاه پارسیان
        public async Task<ActionResult> ParsianCallBack()
        {
            ViewData["BankName"] = "درگاه بانک پارسیان";

            // فراخوانی متد برگشت از درگاه
            await ParsianReturn();

            return View("Return");
        }
        #endregion

        #region اکشن برگشت از درگاه سامان
        public async Task<ActionResult> SamanCallBack()
        {
            ViewData["BankName"] = "درگاه بانک سامان";

            // فراخوانی متد برگشت از درگاه
            await SamanReturn();

            return View("Return");
        }
        #endregion

        #region اکشن برگشت از درگاه ملت
        public async Task<ActionResult> MellatCallBack()
        {
            ViewData["BankName"] = "درگاه بانک ملت";

            // فراخوانی متد برگشت از درگاه
            await MellatReturn();

            return View("Return");
        }
        #endregion        

        /**************متدهای ثبت و ویرایش اطلاعات پرداخت*******************/
        #region ثبت اطلاعات اولیه پرداخت در دیتابیس

        private int InsertPayment(long price, string bankName)
        {
            int paymentId = 0;
            try
            {

                var payment = new Epayment();

                // مبلغ پرداخت
                payment.Amount = price;

                // -100 
                // را خودمان به صورت دستی به کد خطاها اضافه می کنیم
                // که متوجه شوید فعلا پرداخت موفق یا ناموفق بودن آن مشخص نشده
                payment.ResCode = "-100";

                // نام بانک انتخاب شده برای پرداخت
                payment.BankName = bankName;

                // فقط در صورتی که این فید ترو باشد پرداخت تایید و موفق بوده است
                payment.PaymentFinished = false;

                // آی دی کاربر درحال پرداخت که ما یک در نظر گرفتیم و شما باید آی دی کاربری که پرداخت را انجام می دهد ثبت کنید
                payment.UserId = 1;

                // ثبت اطلاعات در دیتابیس
                _epaymentRepository.Add(payment);

                // شماره پرداخت که همان آی دی جدول می باشد که به بانک ارسال می کنیم و هنگام بازگشت اطلاعات را از دیتابیس پیدا می کنیم
                paymentId = payment.PaymentId;

            }
            catch (Exception ex)
            {
                // خطا
            }

            // ارسال شماره سفارش
            return paymentId;
        }
        #endregion

        #region متد ویرایش پرداخت 
        private bool UpdatePayment(int paymentId, string resCode, string description, string retrivalRefNo, string systemTraceNo, long? rrn, bool paymentFinished = false)
        {

            var payment = _epaymentRepository.Find(paymentId);

            if (payment != null)
            {
                if (resCode != null)
                {
                    payment.ResCode = resCode;
                }

                if (description != null)
                {
                    payment.Description = description;
                }

                if (retrivalRefNo != null)
                {
                    payment.RetrivalRefNo = retrivalRefNo;
                }

                if (systemTraceNo != null)
                {
                    payment.SystemTraceNo = systemTraceNo;
                }

                if (rrn != null)
                {
                    payment.Rrn = (long)rrn;
                }

                payment.PaymentFinished = paymentFinished;

                _epaymentRepository.Update(payment);

                return true;
            }
            else
            {
                // اطلاعاتی از دیتابیس پیدا نشد
            }
            return false;
        }
        #endregion

        /*********************متدهای درگاه بانک ملی***********************/
        #region متد پرداخت از درگاه ملی
        private async Task<bool> MelliPayment(long price, string redirectPage, int paymentId)
        {
            try
            {

                // سه مورد زیر توسط بانک به شما ارسال می شود
                // اطلاعات زیر برای تست درگاه بانک می باشد

                // شماره پذیرنده
                string merchantId = "000000140212149";
                // شماره ترمینال
                string terminalId = "24000615";
                // کد امنیتی
                string merchantKey = "K4wIAhAAeOLeHa0kTSkOyLZShuKCoiBF";

                TripleDESCryptoServiceProvider symmetricTripleDES = new TripleDESCryptoServiceProvider();

                var dataBytes = Encoding.UTF8.GetBytes(string.Format("{0};{1};{2}", terminalId, paymentId, price));

                symmetricTripleDES.Mode = CipherMode.ECB;
                symmetricTripleDES.Padding = PaddingMode.PKCS7;

                var encryptor = symmetricTripleDES.CreateEncryptor(Convert.FromBase64String(merchantKey), new byte[8]);

                // رمز نگاری اطلاعات با مرچنت کی
                var signData = Convert.ToBase64String(encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length));



                // آدرس ای پی آی پرداخت 
                var ipgUri = "https://sadad.shaparak.ir/VPG/api/v0/Request/PaymentRequest";

                // اطلاعات زیز به آدرس بالا ارسال خواهد شد
                var data = new
                {
                    TerminalId = terminalId,
                    MerchantId = merchantId,
                    Amount = price,
                    SignData = signData,
                    ReturnUrl = redirectPage,
                    LocalDateTime = DateTime.Now,
                    OrderId = paymentId,
                    //MultiplexingData = MultiplexingData
                };

                // فراخوانی متد درخواست پرداخت
                Models.Melli.PayResultData res = await CallApiPayRequest<Models.Melli.PayResultData>(ipgUri, data);

                if (res != null)
                {
                    // در صورتی که رسکود 0 باشد یعنی توکن توسط بانک ارسال شده و امکان اتصال به بانک وجود 
                    if (res.ResCode == "0")
                    {
                        // اضافه کردن توکن ارسالی بانک به آدرس زیر و سپس هدایت کردن کاربر به صفحه پرداخت
                        Response.Redirect($"https://sadad.shaparak.ir/VPG/Purchase/Index?token={res.Token}");
                    }
                    else
                    {
                        // ثبت اطلاعات خطا در دیتابیس
                        UpdatePayment(paymentId, res.ResCode, res.Description, null, null, null, false);

                        ViewData["Message"] = res.Description;
                    }
                }
                else
                {
                    ViewData["Message"] = "در حال حاظر امکان اتصال به این درگاه وجود ندارد ";
                }
            }
            catch (Exception ex)
            {
                ViewData["Message"] = "در حال حاظر امکان اتصال به این درگاه وجود ندارد ";
            }

            return false;
        }
        #endregion

        #region متد برگشت از درگاه ملی
        private async Task<bool> MelliReturn(PurchaseResult result)
        {
            try
            {
                // بررسی اطلاعات دریافتی
                if (result == null || String.IsNullOrEmpty(result.OrderId) || String.IsNullOrEmpty(result.Token) || String.IsNullOrEmpty(result.ResCode))
                {
                    ViewData["SaleReferenceId"] = " * *************";
                    ViewData["Image"] = "~/Images/notaccept.png";
                    ViewData["Message"] = "اطلاعات پرداخت پیدا نشد";

                    return false;
                }

                // دریافت شماره سفارش
                int paymentId = Int32.Parse(result.OrderId);

                // بررسی می کنیم که اگر رسکود 0 نبود یعنی خطای هنگام پرداخت توسط خریدار اتفاق افتاده است
                if (result.ResCode != "0")
                {
                    ViewData["SaleReferenceId"] = " * *************";
                    ViewData["Image"] = "~/Images/notaccept.png";

                    // تراکنش به دلايلی ناموفق بود.نمايش پیام ناموفق بودن تراکنش
                    if (result.ResCode == "-1")
                    {
                        ViewData["SaleReferenceId"] = " * *************";
                        ViewData["Image"] = "~/Images/notaccept.png";
                        ViewData["Message"] = "پرداخت با موفقیت انجام نشد ";
                    }
                    else
                    {
                        ViewData["SaleReferenceId"] = " * *************";
                        ViewData["Image"] = "~/Images/notaccept.png";
                        ViewData["Message"] = "خطای نامشخص در پرداخت اتفاق افتاد";
                    }

                    // ثبت اطلاعات خطا در دیتابیس
                    UpdatePayment(paymentId, result.ResCode, null, null, null, null, false);

                    return false;
                }

                // سه مورد زیر توسط بانک به شما ارسال می شود
                // اطلاعات زیر برای تست درگاه بانک می باشد

                string merchantId = "000000140212149";
                // شماره ترمینال
                string terminalId = "24000615";
                // کد امنیتی 
                string merchantKey = "K4wIAhAAeOLeHa0kTSkOyLZShuKCoiBF";

                var dataBytes = Encoding.UTF8.GetBytes(result.Token);

                TripleDESCryptoServiceProvider symmetricTripleDES = new TripleDESCryptoServiceProvider();


                symmetricTripleDES.Mode = CipherMode.ECB;
                symmetricTripleDES.Padding = PaddingMode.PKCS7;

                var encryptor = symmetricTripleDES.CreateEncryptor(Convert.FromBase64String(merchantKey), new byte[8]);

                // رمز نگاری اطلاعات با مرچنت کی
                var signedData = Convert.ToBase64String(encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length));


                // آدرس ای پی آی تایید اطلاعات پرداخت 
                var ipgUri = "https://sadad.shaparak.ir/VPG/api/v0/Advice/Verify";

                // اطلاعات زیز به آدرس بالا ارسال خواهد شد
                var data = new
                {
                    token = result.Token,
                    SignData = signedData
                };

                // فراخوانی متد تایید پرداخت
                VerifyResultData res = await CallApiPayVerify<VerifyResultData>(ipgUri, data);

                if (res != null)
                {
                    if (res.ResCode == "0")
                    {
                        // در اینجا پرداخت با موفقیت انجام شده است
                        UpdatePayment(paymentId, result.ResCode, res.Description, res.RetrivalRefNo, res.SystemTraceNo, null, true);

                        ViewData["SaleReferenceId"] = res.RetrivalRefNo;
                        ViewData["Image"] = "~/Images/accept.png";
                        ViewData["Message"] = res.Description;
                    }
                    else
                    {
                        // ثبت اطلاعات خطا در دیتابیس
                        UpdatePayment(paymentId, result.ResCode, res.Description, null, null, null, false);

                        ViewData["SaleReferenceId"] = " * *************";
                        ViewData["Image"] = "~/Images/notaccept.png";
                        ViewData["Message"] = res.Description;
                    }

                }
                else
                {
                    // ثبت اطلاعات خطا در دیتابیس
                    UpdatePayment(paymentId, result.ResCode, "تایید اطلاعات پرداخت انجام نشد", null, null, null, false);

                    ViewData["SaleReferenceId"] = " * *************";
                    ViewData["Image"] = "~/Images/notaccept.png";
                    ViewData["Message"] = "تایید اطلاعات پرداخت انجام نشد";

                }


            }
            catch (Exception ex)
            {
                ViewData["SaleReferenceId"] = " * *************";
                ViewData["Image"] = "~/Images/notaccept.png";
                ViewData["Message"] = "مشکلی در پرداخت به وجود آمده است ، در صورتیکه وجه پرداختی از حساب بانکی شما کسر شده است آن مبلغ به صورت خودکار برگشت داده خواهد شد";
            }

            return false;
        }
        #endregion


        #region CallApiPayRequest متد فراخوانی ای پی آی درخواست پرداخت
        public static async Task<Models.Melli.PayResultData> CallApiPayRequest<T>(string apiUrl, object value)
        {
            // برای جلوگیری از خطای SSL
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();

                // تبدیل اطلاعات ارسال به جیسون
                var json = JsonConvert.SerializeObject(value);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // ارسال اطلاعات به آدری فوق
                var w = client.PostAsync(apiUrl, content);
                w.Wait();

                // دریافت نتیجه پرداخت
                HttpResponseMessage response = w.Result;

                // باشد یعنی اطلاعات ارسال ما توسط بانک دریافت شده است و باید نتیجه آن را بررسی کنیم True برابر IsSuccessStatusCode در صورتی که 
                if (response.IsSuccessStatusCode)
                {
                    // خواندن داده های دریافتی
                    var data = await response.Content.ReadAsStringAsync();

                    // دیسرلایز کردن اطلاعات دریافتی
                    Models.Melli.PayResultData resultData = JsonConvert.DeserializeObject<Models.Melli.PayResultData>(data);

                    return resultData;
                }
                return null;
            }
        }
        #endregion

        #region CallApiPayVerify متد فراخوانی ای پی آی تایید پرداخت
        public static async Task<VerifyResultData> CallApiPayVerify<T>(string apiUrl, object value)
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();

                var json = JsonConvert.SerializeObject(value);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var w = client.PostAsync(apiUrl, content);
                w.Wait();

                HttpResponseMessage response = w.Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    VerifyResultData resultData = JsonConvert.DeserializeObject<VerifyResultData>(data);

                    return resultData;
                }
                return null;
            }
        }
        #endregion

        /*********************متدهای درگاه زرین پال***********************/
        #region متد پرداخت از درگاه زرین پال
        private async Task<bool> ZarinpalPayment(int price, string redirectPage, int paymentId)
        {
            try
            {

                var zp = new Zarinpal.Payment("8b7be794-0a49-11e6-806b-005056a205be", price / 10);
                Zarinpal.Models.PaymentRequestResponse response = await zp.PaymentRequest("خرید", redirectPage, null, null);


                if (response.Status == 100)
                {
                    UpdatePayment(paymentId, null, null, response.Authority, null, null, false);

                    // اتصال به درگاه
                    Response.Redirect(response.Link);
                }
                else
                {
                    UpdatePayment(paymentId, response.Status.ToString(), null, null, null, null, false);
                    ViewData["message"] = PayResult.ZarinPal(response.Status.ToString());
                }

            }
            catch (Exception ex)
            {
                ViewData["message"] = "در حال حاظر امکان اتصال به این درگاه وجود ندارد ";
            }

            return false;
        }
        #endregion

        #region متد برگشت از درگاه زرین پال
        private async Task<bool> ZarinpalReturn(ZarinpalPurchaseResult result)
        {
            try
            {
                // بررسی اطلاعات دریافتی
                if (result == null || String.IsNullOrEmpty(result.OrderId) || String.IsNullOrEmpty(result.Authority) || String.IsNullOrEmpty(result.Status))
                {
                    ViewData["SaleReferenceId"] = " * *************";
                    ViewData["Image"] = "~/Images/notaccept.png";
                    ViewData["Message"] = "اطلاعات پرداخت پیدا نشد";

                    return false;
                }

                // دریافت شماره سفارش
                int paymentId = Int32.Parse(result.OrderId);

                // بررسی می کنیم که اگر رسکود 0 نبود یعنی خطای هنگام پرداخت توسط خریدار اتفاق افتاده است
                if (result.Status.Equals("OK"))
                {
                    // پیدا کردن مبلغ پرداختی از دیتابیس 
                    int amount = FindAmountPayment(paymentId);

                    // تبدیل مبلغ به تومان
                    amount = amount / 10;

                    var zp = new Zarinpal.Payment("8b7be794-0a49-11e6-806b-005056a205be", amount);
                    Zarinpal.Models.PaymentVerificationResponse response = await zp.Verification(result.Authority);


                    // تراکنش به دلايلی ناموفق بود.نمايش پیام ناموفق بودن تراکنش
                    if (response.Status == 100)
                    {
                        // ویرایش اطلاعات پرداخت موفق
                        UpdatePayment(paymentId, response.Status.ToString(), null, null, response.RefId.ToString(), null, true);

                        ViewData["SaleReferenceId"] = response.RefId;
                        ViewData["Image"] = "~/Images/accept.png";
                        ViewData["Message"] = "پرداخت با موفقیت انجام شد ";
                    }
                    else
                    {
                        // ثبت اطلاعات خطا در دیتابیس
                        UpdatePayment(paymentId, response.Status.ToString(), null, null, null, null, false);

                        ViewData["SaleReferenceId"] = " * *************";
                        ViewData["Image"] = "~/Images/notaccept.png";
                        ViewData["Message"] = "خطای نامشخص در پرداخت اتفاق افتاد";
                    }
                }
                else
                {
                    // ثبت اطلاعات خطا در دیتابیس
                    UpdatePayment(paymentId, result.Status, null, null, null, null, false);

                    ViewData["SaleReferenceId"] = " * *************";
                    ViewData["Image"] = "~/Images/notaccept.png";
                    ViewData["Message"] = "تایید اطلاعات پرداخت انجام نشد";

                }
            }
            catch (Exception ex)
            {
                ViewData["SaleReferenceId"] = " * *************";
                ViewData["Image"] = "~/Images/notaccept.png";
                ViewData["Message"] = "مشکلی در پرداخت به وجود آمده است ، در صورتیکه وجه پرداختی از حساب بانکی شما کسر شده است آن مبلغ به صورت خودکار برگشت داده خواهد شد";
            }

            return false;
        }
        #endregion

        #region پیدا کردن مبلغ پرداختی 
        private int FindAmountPayment(int paymentId)
        {

            var payment = _epaymentRepository.Find(paymentId);

            if (payment != null)
            {

                return (int)payment.Amount;
            }
            else
            {
                // اطلاعاتی از دیتابیس پیدا نشد
                return 0;
            }
        }
        #endregion

        /*********************متدهای درگاه پی***********************/
        #region متد پرداخت از درگاه واسط پی
        private async Task<bool> PayIrPayment(long price, string redirectPage, int paymentId)
        {
            try
            {

                // شماره پذیرنده
                string api = "test";

                // آدرس ای پی آی پرداخت 
                var ipgUri = "https://pay.ir/payment/send";

                // اطلاعات زیز به آدرس بالا ارسال خواهد شد
                var data = new
                {
                    api = api,
                    amount = price,
                    redirect = redirectPage,
                    mobile = 09193486512,
                    factorNumber = paymentId,
                };

                // فراخوانی متد درخواست پرداخت
                Models.Pay.PayResultData res = await WebApiService.PostData<Models.Pay.PayResultData>(ipgUri, data);

                if (res != null)
                {
                    // در صورتی که رسکود 1 باشد یعنی ترنس آی دی توسط بانک ارسال شده و امکان اتصال به بانک وجود 
                    if (res.Status == 1)
                    {
                        UpdatePayment(paymentId, null, null, null, res.TransId, null, false);

                        // اضافه کردن توکن ارسالی بانک به آدرس زیر و سپس هدایت کردن کاربر به صفحه پرداخت
                        Response.Redirect($"https://pay.ir/payment/gateway/{res.TransId}");
                    }
                    else
                    {
                        // ثبت اطلاعات خطا در دیتابیس
                        UpdatePayment(paymentId, res.ErrorCode, res.ErrorMessage, null, null, null, false);

                        ViewData["Message"] = res.ErrorMessage;
                    }
                }
                else
                {
                    ViewData["message"] = "در حال حاظر امکان اتصال به این درگاه وجود ندارد ";
                }
            }
            catch (Exception ex)
            {
                ViewData["message"] = "در حال حاظر امکان اتصال به این درگاه وجود ندارد ";
            }

            return false;
        }
        #endregion

        #region متد برگشت از درگاه واسط پی
        private async Task<bool> PayIrReturn(PayPurchaseResult result)
        {
            try
            {
                // بررسی اطلاعات دریافتی
                if (result == null || result.Status == null || result.TransId == null || String.IsNullOrEmpty(result.FactorNumber) || String.IsNullOrEmpty(result.Message))
                {
                    ViewData["SaleReferenceId"] = " * *************";
                    ViewData["Image"] = "~/Images/notaccept.png";
                    ViewData["Message"] = "اطلاعات پرداخت پیدا نشد";

                    return false;
                }

                // دریافت شماره سفارش
                int paymentId = Int32.Parse(result.FactorNumber);

                // بررسی می کنیم که اگر رسکود 0 نبود یعنی خطای هنگام پرداخت توسط خریدار اتفاق افتاده است
                if (result.Status != 1)
                {
                    ViewData["SaleReferenceId"] = " * *************";
                    ViewData["Image"] = "~/Images/notaccept.png";
                    ViewData["Message"] = "پرداخت ناموفق بود";

                    // ثبت اطلاعات خطا در دیتابیس
                    UpdatePayment(paymentId, result.Status.ToString(), result.Message, null, result.TransId.ToString(), null, false);

                    return false;
                }
                // کد امنیتی 
                string api = "test";


                // آدرس ای پی آی تایید اطلاعات پرداخت 
                var ipgUri = " https://pay.ir/payment/verify ";

                // اطلاعات زیز به آدرس بالا ارسال خواهد شد
                var data = new
                {
                    api = api,
                    transId = result.TransId
                };

                // فراخوانی متد تایید پرداخت
                Models.Pay.PayResultData res = await WebApiService.PostData<Models.Pay.PayResultData>(ipgUri, data);

                if (res != null)
                {
                    if (res.Status == 1)
                    {
                        // در اینجا پرداخت با موفقیت انجام شده است
                        UpdatePayment(paymentId, result.Status.ToString(), result.Message, null, result.TransId.ToString(), null, true);

                        ViewData["SaleReferenceId"] = result.TransId;
                        ViewData["Image"] = "~/Images/accept.png";
                        ViewData["Message"] = "پرداخت با موفقیت انجام شد";
                    }
                    else
                    {
                        // ثبت اطلاعات خطا در دیتابیس
                        UpdatePayment(paymentId, result.Status.ToString(), result.Message, null, result.TransId.ToString(), null, false);

                        ViewData["SaleReferenceId"] = " * *************";
                        ViewData["Image"] = "~/Images/notaccept.png";
                        ViewData["Message"] = res.ErrorMessage;
                    }

                }
                else
                {
                    // ثبت اطلاعات خطا در دیتابیس
                    UpdatePayment(paymentId, null, "خطا در ارتباط با سرور درگاه", null, result.TransId.ToString(), null, false);

                    ViewData["SaleReferenceId"] = " * *************";
                    ViewData["Image"] = "~/Images/notaccept.png";
                    ViewData["Message"] = "تایید اطلاعات پرداخت انجام نشد";

                }
            }
            catch (Exception ex)
            {
                ViewData["SaleReferenceId"] = " * *************";
                ViewData["Image"] = "~/Images/notaccept.png";
                ViewData["Message"] = "مشکلی در پرداخت به وجود آمده است ، در صورتیکه وجه پرداختی از حساب بانکی شما کسر شده است آن مبلغ به صورت خودکار برگشت داده خواهد شد";
            }

            return false;
        }
        #endregion

        /*********************متدهای درگاه بانک پارسیان***********************/
        #region پیدا کردن توکن
        private long FindTokenPayment(int paymentId)
        {

            var payment = _epaymentRepository.Find(paymentId);

            if (payment != null)
            {
                return Convert.ToInt64(payment.Token);
            }

            return 0;
        }
        #endregion

        #region متد پرداخت از درگاه پارسیان
        private async Task ParsianPayment(long price, string redirectPage, int paymentId)
        {
            try
            {

                // ایجاد یک شی از 
                //ParsianServiceReference
                //برای پرداخت
                var saleData = new BankParsian.ClientSaleRequestData();

                // پین که توسط بانک پارسیان به شما داده می شود
                saleData.LoginAccount = "71k546xY41Du34L8YH4e"; // 1- پین
                saleData.Amount = price; // 2- مبلغ
                saleData.OrderId = paymentId;  // 3- شمار سفارش
                saleData.CallBackUrl = redirectPage;   // 4- آدرس برگشت
                                                       // اطلاعات اضافی در صورت نیاز
                                                       //parsianPayment.AdditionalData = "";

                // ایجاد یک شی از سرویس فوق

                var saleService = new BankParsian.SaleServiceSoapClient(SaleServiceSoapClient.EndpointConfiguration.SaleServiceSoap);
                var requestResult = await saleService.SalePaymentRequestAsync(saleData);


                // بررسی وجود اطلاعات ارسال شده از درگاه بانک
                if (requestResult.Body.SalePaymentRequestResult.Status == 0 && requestResult.Body.SalePaymentRequestResult.Token > 0)
                {
                    // ویرایش اطلاعات در دیتابیس
                    UpdatePayment(paymentId, null, null, requestResult.Body.SalePaymentRequestResult.Token.ToString(), null, null, false);

                    // اتصال به درگاه بانک
                    Response.Redirect("https://pec.shaparak.ir/NewIPG/?Token=" + requestResult.Body.SalePaymentRequestResult.Token);
                }
                else
                {
                    // ویرایش اطلاعات ارسالی از بانک در صورت عدم اتصال
                    UpdatePayment(paymentId, requestResult.Body.SalePaymentRequestResult.Status.ToString(), requestResult.Body.SalePaymentRequestResult.Message, null, null, null, false);

                    // بانک پیام علت عدم اتصال به بانک را هم ارسال می کند که می توانید به کاربر نمایش دهید
                    // استفاده کنید PaymentResult  یا اینکه از کلاس
                    //requestResult.Result.Body.SalePaymentRequestResult.Message


                    // نمایش خطا به کاربر                    
                    ViewData["Message"] = Models.PaymentResult.PayResult.Parsian(requestResult.Body.SalePaymentRequestResult.Status.ToString());
                }

            }
            catch (Exception ex)
            {
                ViewData["message"] = "در حال حاظر امکان اتصال به این درگاه وجود ندارد ";
            }
        }
        #endregion

        #region متد برگشت از درگاه پارسیان
        private async Task ParsianReturn()
        {
            try
            {
                var tokenStr = Request.Form["Token"];
                var statusStr = Request.Form["status"];

                // است paymentId همان orderIdStr  
                var orderIdStr = Request.Form["OrderId"];
                var terminalNoStr = Request.Form["TerminalNo"];
                var RRNStr = Request.Form["RRN"];
                var hashCardNumberStr = Request.Form["HashCardNumber"];
                var amountStr = Request.Form["Amount"];

                if (!String.IsNullOrEmpty(tokenStr) && !String.IsNullOrEmpty(statusStr))
                {
                    long token = long.Parse(tokenStr);

                    if (statusStr == "0" && token > 0)
                    {
                        // برای تایید پرداخت BankParsianConfirmPayment ایجاد شی از                         
                        var confirmRequestData = new BankParsianConfirmPayment.ClientConfirmRequestData();

                        // پین که توسط بانک پارسیان به شما داده می شود
                        confirmRequestData.LoginAccount = "";

                        int paymentId = Convert.ToInt32(orderIdStr);

                        //  توکنی که هنگام اتصال به درگاه در دیتابیس ذخیره کرده بودیم را باید از دیتابیس پیدا کنیم
                        confirmRequestData.Token = FindTokenPayment(paymentId);

                        var confirmService =
                            new BankParsianConfirmPayment.ConfirmServiceSoapClient(ConfirmServiceSoapClient
                                .EndpointConfiguration.ConfirmServiceSoap);

                        // تایید پرداحت
                        var confirmResult = await confirmService.ConfirmPaymentAsync(confirmRequestData);

                        if (confirmResult.Body.ConfirmPaymentResult.Status == 0 && confirmResult.Body.ConfirmPaymentResult.RRN > 0)
                        {
                            // ویرایش اطلاعات پرداخت موفق
                            UpdatePayment(paymentId, confirmResult.Body.ConfirmPaymentResult.Status.ToString(), PayResult.Parsian(confirmResult.Body.ConfirmPaymentResult.Status.ToString()), null, terminalNoStr, confirmResult.Body.ConfirmPaymentResult.RRN, true);

                            // قرار دادن اطلاعات پرداخت در ویوبگ ها
                            ViewData["Message"] = "پرداخت با موفقیت انجام شد";
                            ViewData["RefrenceNumber"] = confirmResult.Body.ConfirmPaymentResult.RRN;
                            ViewData["Image"] = "~/Images/accept.png";
                        }
                        // پرداخت نا موفق بوده
                        else
                        {
                            // نکته مهم
                            // در اینجا نیاز به ریورسال کردن پرداخت نمی باشد و در صورت ناموفق بودن خرید مبلغ به صورت خودکار برگشت داده می شود

                            // ویرایش اطلاعات پرداخت ناموفق
                            UpdatePayment(paymentId, confirmResult.Body.ConfirmPaymentResult.Status.ToString(), PayResult.Parsian(confirmResult.Body.ConfirmPaymentResult.Status.ToString()), null, terminalNoStr, null, false);


                            // نمایش اطلاعات خرید و وضعیت خرید به خریدار
                            ViewData["Message"] = PayResult.Parsian(confirmResult.Body.ConfirmPaymentResult.Status.ToString());
                            ViewData["RefrenceNumber"] = " * *************";
                            ViewData["Image"] = "~/Images/notaccept.png";

                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(orderIdStr))
                        {
                            int paymentId = Convert.ToInt32(orderIdStr);

                            UpdatePayment(paymentId, statusStr, null, null, null, null, false);
                        }

                        // نمایش اطلاعات خرید و وضعیت خرید به خریدار
                        ViewData["Message"] = Models.PaymentResult.PayResult.Parsian(statusStr);
                        ViewData["RefrenceNumber"] = " * *************";
                        ViewData["Image"] = "~/Images/notaccept.png";
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(orderIdStr))
                    {
                        int paymentId = Convert.ToInt32(orderIdStr);

                        UpdatePayment(paymentId, statusStr, null, null, null, null, false);
                    }

                    // نمایش اطلاعات خرید و وضعیت خرید به خریدار
                    ViewData["Message"] = "پاسخی از درگاه بانکی دریافت نشد";
                    ViewData["RefrenceNumber"] = " * *************";
                    ViewData["Image"] = "~/Images/notaccept.png";
                }
            }
            catch (Exception ex)
            {
                ViewData["SaleReferenceId"] = " * *************";
                ViewData["Image"] = "~/Images/notaccept.png";
                ViewData["Message"] = "مشکلی در پرداخت به وجود آمده است ، در صورتیکه وجه پرداختی از حساب بانکی شما کسر شده است آن مبلغ به صورت خودکار برگشت داده خواهد شد";

            }
        }
        #endregion

        #region برگشت دادن مبلغ به حساب کاربر

        /// <summary>
        /// از این تابع فقط در شرایطی که سیاست سایت ایجاد می کند که بعد از نهایی کردن خرید باز نیاز باشد که مبلغ در همان روز به حساب کاربر برگشت داده شود استفاده کنید
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="loginAccount"></param>
        /// <returns></returns>
        private bool ReversalPayment(int paymentId, string loginAccount)
        {

            var reversalRequestData = new BankParsianReversal.ClientReversalRequestData();

            // پین که توسط بانک پارسیان به شما داده می شود
            reversalRequestData.LoginAccount = loginAccount;

            //  توکنی که هنگام اتصال به درگاه در دیتابیس ذخیره کرده بودیم را باید از دیتابیس پیدا کنیم
            reversalRequestData.Token = FindTokenPayment(paymentId);

            var reversalService =
                new BankParsianReversal.ReversalServiceSoapClient(ReversalServiceSoapClient.EndpointConfiguration
                    .ReversalServiceSoap);

            // برگشت دادن مبلغ
            var reversalServiceResult = reversalService.ReversalRequestAsync(reversalRequestData);

            return reversalServiceResult.Status == 0;

            // اگه از این متدد استفاده کنید باید در دیتابیس هم این مورد را ویرایش کنید که برگشت مبلغ اتفاق افتاده است
        }

        #endregion

        /*********************متدهای درگاه بانک سامان***********************/
        #region متد پرداخت از درگاه سامان
        private async Task SamanPayment(long price, string redirectPage, int paymentId)
        {
            try
            {
                // شماره ترمینال
                string termId = "110545435";

                // ایجاد یک شی از 
                //SepInitPayment
                //برای پرداخت
                var initPayment = new BankSaman.PaymentIFBindingSoapClient(BankSaman.PaymentIFBindingSoapClient.EndpointConfiguration.PaymentIFBindingSoap);

                // ارسال اطلاعات به درگاه بانک به روش توکن
                // 1- شماره ترمینال
                // 2- شماره پرداخت
                // 3- مبلغ
                // 5- مبلغ
                // 6-مبلغ
                // 7-مبلغ
                // 8-مبلغ
                // 9-مبلغ
                // 10- اطلاعات اضافی
                // 11- اطلاعات اضافی
                // 12- 0
                string token = await initPayment.RequestTokenAsync(termId, paymentId.ToString(), long.Parse(price.ToString()), 0, 0, 0, 0, 0, 0, "", "", 0);

                // بررسی وجود توکن ارسال شده از درگاه بانک
                if (!String.IsNullOrEmpty(token))
                {
                    if (String.IsNullOrEmpty(PayResult.Saman(token)))
                    {
                        // اگر عدد برگشت مانند -18 آی پی بسته است و.. به کلاس 
                        //SepResult
                        //مراجعه شود

                        // ثبت توکن در دیتابیس
                        UpdatePayment(paymentId, null, null, token, null, null, false);

                        // ایجاد یک شی از نیم ولیو کالکشن
                        NameValueCollection datacollection = new NameValueCollection();

                        // اضافه کردن توکن به شی ساخت شده از نیم ولیو کالکشن
                        datacollection.Add("Token", token);

                        // اضافه کردن آدرس برگشت از درگاه در به شی ساخت شده از نیم ولیو کالکشن
                        datacollection.Add("RedirectURL", redirectPage);

                        // ارسال اطلاعات به درگاه
                        await Response.WriteAsync(HttpHelper.PreparePOSTForm("https://sep.shaparak.ir/payment.aspx", datacollection));
                    }
                    else
                    {
                        // فرا خوانی متد آپدیت پی منت برای ویرایش اطلاعات ارسالی از درگاه در صورت عدم اتصال
                        UpdatePayment(paymentId, token, PayResult.Saman(token), null, null, null, false);

                        // نمایش خطا به کاربر
                        ViewData["message"] = PayResult.Saman(token);
                    }
                }
                else
                {
                    // نمایش خطا به کاربر
                    ViewData["message"] = "در حال حاضر امکان پرداخت وجود ندارد";
                }
            }
            catch (Exception ex)
            {
                ViewData["message"] = "در حال حاظر امکان اتصال به این درگاه وجود ندارد ";
            }
        }
        #endregion

        #region متد برگشت از درگاه سامان
        private async Task SamanReturn()
        {
            try
            {
                // بررسی وجود استیت ارسالی از درگاه
                // در صورت عدم وجود خطا را نمایش می دهیم
                if (Request.Form["state"].ToString().Equals(string.Empty))
                {
                    //ViewData["Message = "خريد شما توسط بانک تاييد شده است اما رسيد ديجيتالي شما تاييد نگشت! مشکلي در فرايند رزرو خريد شما پيش آمده است";
                    ViewData["message"] = "پاسخی از درگاه بانکی دریافت نشد";
                    ViewData["SaleReferenceId"] = " * *************";
                    ViewData["Image"] = "~/Images/notaccept.png";
                }

                // بررسی وجود رف نام ارسالی از درگاه
                // در صورت عدم وجود خطا را نمایش می دهیم
                // RefNum همان paymentId است
                else if (Request.Form["RefNum"].ToString().Equals(string.Empty) && Request.Form["state"].ToString().Equals(string.Empty))
                {
                    ViewData["message"] = "فرايند انتقال وجه با موفقيت انجام شده است اما فرايند تاييد رسيد ديجيتالي با خطا مواجه گشت";
                    ViewData["SaleReferenceId"] = " * *************";
                    ViewData["Image"] = "~/Images/notaccept.png";
                }

                // بررسی وجود رس نام ارسالی از درگاه
                // در صورت عدم وجود خطا را نمایش می دهیم
                else if (Request.Form["ResNum"].ToString().Equals(string.Empty) && Request.Form["state"].ToString().Equals(string.Empty))
                {
                    ViewData["message"] = "خطا در برقرار ارتباط با بانک";
                    ViewData["SaleReferenceId"] = " * *************";
                    ViewData["Image"] = "~/Images/notaccept.png";
                }
                else
                {
                    // تغییر های مورد تعریف شده برای قرار دادن اطلاعات دریافتی از درگاه
                    string refrenceNumber = string.Empty;

                    // این همان PaymentId است
                    string reservationNumber = string.Empty;
                    string transactionState = string.Empty;
                    string traceNumber = string.Empty;


                    // کد سفارش که به صورت عدد و حروف می باشد
                    refrenceNumber = Request.Form["RefNum"].ToString();

                    // کد ارسالی از طرف سایت که شماره آی دی همان اطلاعات ثبتی در هنگام اتصال به درگاه
                    // این همان PaymentId است
                    reservationNumber = Request.Form["ResNum"].ToString();

                    // وضعیت پرداخت
                    transactionState = Request.Form["state"].ToString();

                    // شماره پیگیری
                    traceNumber = Request.Form["TraceNo"].ToString();

                    int paymentId = Convert.ToInt32(reservationNumber);

                    if (transactionState.Equals("OK"))
                    {
                        // در صورت نیاز می توانیم بررسی کنیم که 
                        // refrenceNumber
                        // که بانک ارسال کرد در دیتابیس تکراری نباشد
                        ///////////////////////////////////////////////////////////////////////////////////
                        //   *** IMPORTANT  ****   ATTENTION
                        // Here you should check refrenceNumber in your DataBase tp prevent double spending
                        ///////////////////////////////////////////////////////////////////////////////////

                        // جلوگیری از خطای اس اس ال
                        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                        // ایجاد یک شی از 
                        //PaymentIFBindingSoapClient
                        //برای پرداخت
                        var srv = new BankSamanVerify.PaymentIFBindingSoapClient(BankSamanVerify.PaymentIFBindingSoapClient.EndpointConfiguration.PaymentIFBindingSoap);

                        // تایید اطلاعات پرداخت و دریافت نتیجه
                        var result = await srv.verifyTransactionAsync(Request.Form["RefNum"], Request.Form["MID"]);

                        // بررسی نتیجه برگشتی از درگاه
                        if (result > 0)
                        {
                            // پیدا کردن مبلغ پرداختی از درگاه
                            long amount = FindAmountPayment(paymentId);

                            // تبدیل مبلغ به ریال
                            //amount = amount * 10;

                            // چک کردن مبلغ بازگشتی از سرویس با مبلغ تراکنش
                            if ((long)result == amount)
                            {
                                // در اینجا خرید موفق بوده و اطلاعات دریافتی از درگاه را در دیتابیس ذخیره می کنیم
                                UpdatePayment(paymentId, transactionState, null, traceNumber, refrenceNumber, null, true);

                                // قرار دادن اطلاعات پرداخت در ویوبگ ها
                                //ViewData["Message = "بانک صحت رسيد ديجيتالي شما را تصديق نمود. فرايند خريد تکميل گشت";
                                ViewData["Message"] = "پرداخت با موفقیت انجام شد.";
                                ViewData["SaleReferenceId"] = traceNumber;
                                ViewData["RefrenceNumber"] = refrenceNumber;
                                ViewData["Image"] = "~/Images/accept.png";
                            }
                            // عدم یکسان بودن مبلغ پرداختی با مبلغ موجود در دیتابیس
                            else
                            {
                                //نام کاربری همان ام آی دی است
                                string userName = Request.Form["MID"];

                                // رمز عبور برای شما توسط سامان کیش ایمیل شده است
                                string pass = "148000261";

                                // فراخوانی متد ریورس ترنزاکشن برای بازگشت دادن مبلغ به حساب خریدار
                                await srv.reverseTransactionAsync(Request.Form["RefNum"], Request.Form["MID"], userName, pass);

                                // پرداخت ناموفق بوده و اطلاعات دریافتی را در دیتابیس ثبت می کنیم
                                UpdatePayment(paymentId, result.ToString(), PayResult.Saman(transactionState), null, refrenceNumber, null, false);

                                // نمایش اطلاعات خرید و وضعیت خرید به خریدار
                                ViewData["message"] = PayResult.Saman(transactionState);
                                ViewData["SaleReferenceId"] = " * *************";
                                ViewData["Image"] = "~/Images/notaccept.png";

                            }
                        }
                        // بعد از وریفای کردن خرید نتیجه بزرگتر از صفر نبود این قسمت اجرا می شود
                        else
                        {
                            // پرداخت ناموفق بوده و اطلاعات دریافتی را در دیتابیس ثبت می کنیم
                            UpdatePayment(paymentId, result.ToString(), PayResult.Saman(transactionState), null, refrenceNumber, null, false);

                            // نمایش اطلاعات خرید و وضعیت خرید به خریدار
                            ViewData["message"] = PayResult.Saman(transactionState);
                            ViewData["SaleReferenceId"] = " * *************";
                            ViewData["Image"] = "~/Images/notaccept.png";
                        }
                    }
                    // در صورتی که 
                    // transactionState
                    // برابر 
                    // ok
                    // نبود این قسمت اجرا می شود
                    else
                    {
                        // پرداخت ناموفق بوده و اطلاعات دریافتی را در دیتابیس ثبت می کنیم
                        UpdatePayment(paymentId, transactionState, null, null, refrenceNumber, null, false);

                        if (!String.IsNullOrEmpty(PayResult.Saman(transactionState)))
                        {
                            ViewData["Message"] = PayResult.Saman(transactionState);
                        }
                        else
                        {
                            ViewData["Message"] = "متاسفانه بانک خريد شما را تاييد نکرده است";
                        }
                        ViewData["SaleReferenceId"] = " * *************";
                        ViewData["Image"] = "~/Images/notaccept.png";

                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["SaleReferenceId"] = " * *************";
                ViewData["Image"] = "~/Images/notaccept.png";
                ViewData["message"] = "مشکلی در پرداخت به وجود آمده است ، در صورتیکه وجه پرداختی از حساب بانکی شما کسر شده است آن مبلغ به صورت خودکار برگشت داده خواهد شد";
            }
        }
        #endregion

        /*********************متدهای درگاه بانک ملت***********************/
        #region متد پرداخت از درگاه ملت
        /// <summary>
        /// این تابع یک درخواست پرداخت را مطابق با اطلاعات ورودی خود ایجاد کرده و با توجه به نتیجه برگشتی درخواست ، عملیات هدایت به درگاه یا نمایش پیغام های مناسب را انجام می دهد.
        /// </summary>
        /// <param name="price"></param>
        /// <param name="redirectPage"></param>
        /// <param name="paymentId"></param>
        private async Task MellatPayment(long price, string redirectPage, int paymentId)
        {
            try
            {
                //فراخوانی متد رفع مشکلات امنیتی
                BypassCertificateError();

                // اطلاعات شماره پذیرنده و... درگاه پرداخت بانک ملت در وب کانفیک ست شده

                //شماره ترمینال اخذ شده از به پرداخت
                string TerminalId = "123";

                //نام کاربری اخذ شده از به پرداخت
                string UserName = "123";

                //رمز عبور اخذ شده از به پرداخت
                string UserPassword = "123";

                var payment = new BankMellat.PaymentGatewayClient();

                //فراخوانی تابع درخواست پرداخت
                var result = await payment.bpPayRequestAsync(
                  Int64.Parse(TerminalId),
                  UserName,
                  UserPassword,
                  paymentId,
                  price,
                  GetDate(),
                  GetTime(),
                  "خرید از سایت ",
                  redirectPage,
                 Int64.Parse("0")
                  );



                //بررسی نتیجه برگشتی ار تابع پرداخت
                //در صورت نول نبودن یعنی فراخوانی انجام شده
                //در صورت نول بودن یعنی فراخوانی انجام نشده
                if (result != null)
                {
                    //پس از فراخوانی صحیح یک رشته از طرف بان ارسال می گررد که حاوی کد ارجاع و کد پیغام از طرف بانک است

                    //است که به صورت زیر تفکیک می گردد A0ds04545sd24545,0 مثلا کد برگشتی به صورت 
                    String[] resultArray = result.Body.@return.Split(',');

                    //در صورتی که کد پیغام 0 باشد یعنی عملیات پرداخت انجام پذیر است و با دستور زیر بررسی می گردد
                    if (int.Parse(resultArray[0].ToString()) == 0)
                    {
                        // ویرایش پرداخت برای ثبت رف آی دی
                        UpdatePayment(paymentId, null, null, resultArray[1], null, null, false);

                        // ایجاد یک شی از نیم ولیو کالکشن
                        NameValueCollection datacollection = new NameValueCollection();

                        // اضافه کردن توکن به شی ساخت شده از نیم ولیو کالکشن
                        datacollection.Add("RefId", resultArray[1]);

                        // ارسال اطلاعات به درگاه
                        await Response.WriteAsync(HttpHelper.PreparePOSTForm("https://bpm.shaparak.ir/pgwchannel/startpay.mellat", datacollection));
                    }
                    else
                    {
                        // فرا خوانی متد آپدیت پی منت برای ویرایش اطلاعات ارسالی از درگاه در صورت عدم اتصال
                        //UpdatePayment(paymentId, resultArray[0].ToString(), 0, null, false);

                        //در صورتی که کد برگشتی 0 نباشد این کد به تابع زیر ارسال شده و پیغامی متناسب با آن نمایش داده می شود
                        ViewData["message"] = PayResult.Mellat(resultArray[0]);
                    }

                }
                else
                {
                    ViewData["message"] = "در حال حاظر امکان اتصال به این درگاه وجود ندارد ";
                }
            }
            catch (Exception ex)
            {
                ViewData["message"] = "در حال حاظر امکان اتصال به این درگاه وجود ندارد ";
                //ViewData["message = "Error: " + ex.Message;
            }
        }
        #endregion

        #region متد بررسی برگشت از بانک ملت
        private async Task MellatReturn()
        {
            // فراخوانی متد رفع پیغام های امنیتی
            BypassCertificateError();

            if (string.IsNullOrEmpty(Request.Form["SaleReferenceId"]))
            {
                if (!string.IsNullOrEmpty(Request.Form["ResCode"]))
                {
                    ViewData["Message"] = PayResult.Mellat(Request.Form["ResCode"]);
                    ViewData["SaleReferenceId"] = "**************";
                    ViewData["Image"] = "~/Images/notaccept.png";
                }
                else
                {
                    ViewData["Message"] = "شماره رسید قابل قبول نیست";
                    ViewData["SaleReferenceId"] = "**************";
                    ViewData["Image"] = "~/Images/notaccept.png";
                }
            }
            else
            {
                try
                {
                    string TerminalId = "123";
                    string UserName = "123";
                    string UserPassword = "123";


                    // این همان paymentId است
                    long SaleOrderId = 0;
                    long SaleReferenceId = 0;
                    string RefId = null;

                    try
                    {
                        // این همان paymentId است
                        SaleOrderId = long.Parse(Request.Form["SaleOrderId"].ToString().TrimEnd());

                        SaleReferenceId = long.Parse(Request.Form["SaleReferenceId"].ToString().TrimEnd());
                        RefId = Request.Form["RefId"].ToString().TrimEnd();
                    }
                    catch (Exception ex)
                    {
                        ViewData["message"] = ex + "<br/>" + " وضعیت:مشکلی در پرداخت به وجود آمده است ، در صورتیکه وجه پرداختی از حساب بانکی شما کسر شده است آن مبلغ به صورت خودکار برگشت داده خواهد شد ";
                        ViewData["SaleReferenceId"] = "**************";
                        ViewData["Image"] = "~/Images/notaccept.png";
                        return;
                    }

                    var bpService = new BankMellat.PaymentGatewayClient();

                    var Vresult = await bpService.bpVerifyRequestAsync(long.Parse(TerminalId), UserName, UserPassword, SaleOrderId, SaleOrderId, SaleReferenceId);

                    if (Vresult != null)
                    {
                        if (Vresult.Body.@return == "0")
                        {
                            var IQresult = await bpService.bpInquiryRequestAsync(long.Parse(TerminalId), UserName, UserPassword, SaleOrderId, SaleOrderId, SaleReferenceId);

                            if (IQresult.Body.@return == "0")
                            {
                                //در اینجا پرداخت انجام شده است و عملیات مربوط به سایت انجام می گیرد

                                int paymentId = Convert.ToInt32(SaleOrderId);

                                // آپدیت کردن اطلاعات پرداخت
                                UpdatePayment(paymentId, Vresult.Body.@return, null, SaleReferenceId.ToString(), RefId, null, true);

                                //ViewData["message = "پرداخت با موفقیت انجام شد." + "<br/>" + "شناسه سفارش: " + SaleOrderId + "<br/>" + " شناسه مرجع تراکنش:" + SaleReferenceId + "<br/>" + "رسید پرداخت:" + RefId;

                                ViewData["Message"] = "پرداخت با موفقیت انجام شد.";
                                ViewData["SaleReferenceId"] = SaleReferenceId;
                                ViewData["Image"] = "~/Images/accept.png";

                                // تایید پرداخت
                                var Sresult = await bpService.bpSettleRequestAsync(long.Parse(TerminalId), UserName, UserPassword, SaleOrderId, SaleOrderId, SaleReferenceId);

                                if (Sresult != null)
                                {
                                    if (Sresult.Body.@return == "0" || Sresult.Body.@return == "45")
                                    {
                                        //تراکنش تایید و ستل شده است 
                                    }
                                    else
                                    {
                                        //تراکنش تایید شده ولی ستل نشده است
                                    }
                                }
                            }
                            else
                            {

                                //عملیات برگشت دادن مبلغ
                                var Rvresult = await bpService.bpReversalRequestAsync(long.Parse(TerminalId), UserName, UserPassword, SaleOrderId, SaleOrderId, SaleReferenceId);

                                ViewData["Message"] = "تراکنش بازگشت داده شد";
                                ViewData["SaleReferenceId"] = "**************";
                                ViewData["Image"] = "~/Images/notaccept.png";


                                int paymentId = Convert.ToInt32(SaleOrderId);

                                // آپدیت اطلاعات پرداخت در دیتابیس
                                UpdatePayment(paymentId, IQresult.Body.@return, null, SaleReferenceId.ToString(), RefId, null, false);
                            }
                        }
                        else
                        {
                            ViewData["Message"] = PayResult.Mellat(Vresult.Body.@return);
                            ViewData["SaleReferenceId"] = "**************";
                            ViewData["Image"] = "~/Images/notaccept.png";

                            int paymentId = Convert.ToInt32(SaleOrderId);

                            // آپدیت اطلاعات پرداخت در دیتابیس
                            UpdatePayment(paymentId, Vresult.Body.@return, SaleReferenceId.ToString(), RefId, null, null, false);
                        }
                    }
                    else
                    {
                        ViewData["Message"] = "شماره رسید قابل قبول نیست";
                        ViewData["SaleReferenceId"] = "**************";
                        ViewData["Image"] = "~/Images/notaccept.png";
                    }

                }
                catch (Exception ex)
                {
                    string errors = ex.Message;
                    ViewData["Message"] = "مشکلی در پرداخت به وجود آمده است ، در صورتیکه وجه پرداختی از حساب بانکی شما کسر شده است آن مبلغ به صورت خودکار برگشت داده خواهد شد";
                    ViewData["SaleReferenceId"] = "**************";
                    ViewData["Image"] = "~/Images/notaccept.png";
                }
            }
        }

        #endregion]

        #region // تابع رفع پیغام های امنیتی
        void BypassCertificateError()
        {
            ServicePointManager.ServerCertificateValidationCallback +=
                delegate (
                    Object sender1,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
        }

        #endregion

        #region // تابع دریافت تاریخ
        protected string GetDate()
        {
            return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') +
                   DateTime.Now.Day.ToString().PadLeft(2, '0');
        }
        #endregion

        #region // تابع دریافت زمان
        protected string GetTime()
        {
            return DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') +
                   DateTime.Now.Second.ToString().PadLeft(2, '0');
        }
        #endregion

    }
}