﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBooking.Models;
using WebBooking.Models.DB;
using System.Data.Entity;
using WebBooking.Models.ViewModel;
using System.Web.Razor.Parser.SyntaxTree;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNet.Identity;
using System.Configuration;
using WebBooking.Models.Payments;

namespace WebBooking.Controllers
{
    [Authorize]
    public class DatPhongController : Controller
    {
        dbHotel db = new dbHotel();
        // GET: Booking
        public ActionResult Index()
        {
            return View();
        }

        // GET: Booking/Create
        public ActionResult DatPhong(int id)
        {
            var room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            var booking = new Booking
            {
                roomid = room.roomid,
                checkin = DateTime.Today,
                checkout = DateTime.Today.AddDays(1),
                bookingdate = DateTime.UtcNow
            };

            // Tạo danh sách số người cho dropdownlist
            List<int> soNguoiList = new List<int> { 1, 2, 3, 4, 5 };

            // Gán danh sách số người vào ViewBag
            ViewBag.SoNguoiList = new SelectList(soNguoiList);



            ViewBag.ThongTinPhong = room;

            //var tupleModel = new Tuple<Booking, Room>(booking, room);

            return View(booking);
        }

        // POST: Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult DatPhong([Bind(Include = "roomid, customername, email, phone, identiyid, birthday, bookingdate, numberpeople, checkin, checkout, total, statusid")] Booking booking)
        {
            //var code = new { Url = "" };
            if (ModelState.IsValid)
            {
                var room = db.Rooms.Find(booking.roomid);
                if (room == null)
                {
                    return HttpNotFound();
                }
                booking.Room = room;
                booking.statusid = 1;
                booking.bookingdate = DateTime.UtcNow;
                booking.userid = User.Identity.GetUserId();
                if (booking.checkin.HasValue && booking.checkout.HasValue)
                {
                    TimeSpan timeSpan = booking.checkout.Value - booking.checkin.Value;
                    booking.total = (int)timeSpan.TotalDays * booking.Room.price;
                }
                db.Bookings.Add(booking);
                db.SaveChanges();
                //send mail
                SendBookingConfirmationEmail(booking);
                SendNewBookingNotificationEmail(booking);
                string contentCustomer = Server.MapPath("~/Content/template/send2.html");

                return RedirectToAction("ChiTietDatPhong", "DatPhong", new { id = booking.bookingid });
            }
            else
            {
                // Nếu ModelState không hợp lệ, thực hiện các xử lý tương ứng với từng lỗi

                // Ví dụ: Kiểm tra lỗi nhập sai thông tin khách hàng
                if (ModelState["customername"].Errors.Count > 0)
                {
                    ViewBag.ErrorMessage = "Vui lòng nhập tên khách hàng.";
                }
                else if (ModelState["email"].Errors.Count > 0)
                {
                    ViewBag.ErrorMessage = "Vui lòng nhập địa chỉ email hợp lệ.";
                }
                else if (ModelState["phone"].Errors.Count > 0)
                {
                    ViewBag.ErrorMessage = "Vui lòng nhập số điện thoại hợp lệ.";
                }
                else if (ModelState["checkin"].Errors.Count > 0)
                {
                    ViewBag.ErrorMessage = "Vui lòng nhập ngày nhận phòng hợp lệ.";
                }
                else if (ModelState["identiyid"].Errors.Count > 0)
                {
                    ViewBag.ErrorMessage = "Vui lòng nhập CMND/CCCD hợp lệ.";
                }
                else if (ModelState["checkout"].Errors.Count > 0)
                {
                    ViewBag.ErrorMessage = "Vui lòng nhập ngày trả phòng hợp lệ.";
                }
                else if (ModelState["birthday"].Errors.Count > 0)
                {
                    ViewBag.ErrorMessage = "Vui lòng nhập ngày sinh hợp lệ.";
                }
                else if (ModelState["numberpeople"].Errors.Count > 0)
                {
                    ViewBag.ErrorMessage = "Vui lòng nhập số người hợp lệ.";
                }
                // Xử lý các lỗi khác tương tự
                //var url = UrlPayment(booking.StatusCredit.statuscreditid, booking.bookingid.ToString());
                // Gán lại các giá trị để hiển thị lại trong view
                var room = db.Rooms.Find(booking.roomid);
                ViewBag.ThongTinPhong = room;
                List<int> soNguoiList = new List<int> { 1, 2, 3, 4, 5 };
                ViewBag.SoNguoiList = new SelectList(soNguoiList);
                return View(booking);
            }

        }

        // GET: Booking/Details/5
        public ActionResult ChiTietDatPhong(int id)
        {
            var booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }


            return View(booking);
        }

        //public ActionResult VnpayReturn()
        //{
        //    if (Request.QueryString.Count > 0)
        //    {
        //        string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
        //        var vnpayData = Request.QueryString;
        //        VnPayLibrary vnpay = new VnPayLibrary();

        //        foreach (string s in vnpayData)
        //        {
        //            //get all querystring data
        //            if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
        //            {
        //                vnpay.AddResponseData(s, vnpayData[s]);
        //            }
        //        }
        //        string orderCode = Convert.ToString(vnpay.GetResponseData("vnp_TxnRef"));
        //        long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
        //        string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
        //        string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
        //        String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
        //        String TerminalID = Request.QueryString["vnp_TmnCode"];
        //        long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
        //        String bankCode = Request.QueryString["vnp_BankCode"];

        //        bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
        //        if (checkSignature)
        //        {
        //            if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
        //            {
        //                var itemOrder = db.Bookings.FirstOrDefault(x => x.bookingid.ToString() == orderCode);
        //                if (itemOrder != null)
        //                {
        //                    itemOrder.StatusCredit.statuscreditid = 2;//đã thanh toán
        //                    db.Bookings.Attach(itemOrder);
        //                    db.Entry(itemOrder).State = System.Data.Entity.EntityState.Modified;
        //                    db.SaveChanges();
        //                }
        //                //Thanh toan thanh cong
        //                ViewBag.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
        //                //log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
        //            }
        //            else
        //            {
        //                //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
        //                ViewBag.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
        //                //log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
        //            }
        //            //displayTmnCode.InnerText = "Mã Website (Terminal ID):" + TerminalID;
        //            //displayTxnRef.InnerText = "Mã giao dịch thanh toán:" + orderId.ToString();
        //            //displayVnpayTranNo.InnerText = "Mã giao dịch tại VNPAY:" + vnpayTranId.ToString();
        //            ViewBag.ThanhToanThanhCong = "Số tiền thanh toán (VND):" + vnp_Amount.ToString();
        //            //displayBankCode.InnerText = "Ngân hàng thanh toán:" + bankCode;
        //        }
        //    }
        //    //var a = UrlPayment(0, "DH3574");
        //    return View();
        //}

        //#region Thanh toán vnpay
        //public string UrlPayment(int TypePaymentVN, string orderCode)
        //{
        //    var urlPayment = "";
        //    var order = db.Bookings.FirstOrDefault(x => x.bookingid.ToString() == orderCode);
        //    //Get Config Info
        //    string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
        //    string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
        //    string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
        //    string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key

        //    //Build URL for VNPAY
        //    VnPayLibrary vnpay = new VnPayLibrary();
        //    var Price = (long)order.total * 100;
        //    vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
        //    vnpay.AddRequestData("vnp_Command", "pay");
        //    vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
        //    vnpay.AddRequestData("vnp_Amount", Price.ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
        //    if (TypePaymentVN == 1)
        //    {
        //        vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
        //    }
        //    else if (TypePaymentVN == 2)
        //    {
        //        vnpay.AddRequestData("vnp_BankCode", "VNBANK");
        //    }
        //    else if (TypePaymentVN == 3)
        //    {
        //        vnpay.AddRequestData("vnp_BankCode", "INTCARD");
        //    }

        //    vnpay.AddRequestData("vnp_CreateDate", order.paymentdate.ToString("yyyyMMddHHmmss"));
        //    vnpay.AddRequestData("vnp_CurrCode", "VND");
        //    vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
        //    vnpay.AddRequestData("vnp_Locale", "vn");
        //    vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng :" + order.bookingid);
        //    vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

        //    vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
        //    vnpay.AddRequestData("vnp_TxnRef", order.bookingid.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

        //    //Add Params of 2.1.0 Version
        //    //Billing

        //    urlPayment = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
        //    //log.InfoFormat("VNPAY URL: {0}", paymentUrl);
        //    return urlPayment;
        //}
        //#endregion


        private void SendNewBookingNotificationEmail(Booking booking)
        {
            // Địa chỉ email và mật khẩu của tài khoản gửi mail
            string senderEmail = "phamphongvt1998@gmail.com";
            string senderPassword = "fckmwwkbkjwbvboh";

            // Địa chỉ email của admin nhận mail
            string adminEmail = "minzondev@gmail.com";

            // Tiêu đề email
            string subject = "Có đơn đặt phòng mới";

            // Tạo nội dung email
            string body = "Bạn nhận được một đơn đặt phòng mới:\n\n";
            body += "Thông tin đặt phòng:\n";
            body += "Phòng: " + booking.Room.roomname + "\n";
            body += "Mã Đặt Phòng: " + booking.bookingid + "\n";
            body += "Khách hàng: " + booking.customername + "\n";
            body += "Email: " + booking.email + "\n";
            body += "Số điện thoại: " + booking.phone + "\n";
            body += "Ngày Nhận Phòng: " + booking.checkin + "\n";
            body += "Ngày Trả Phòng: " + booking.checkout + "\n";
            // Thêm các thông tin khác tùy ý

            // Tạo đối tượng MailMessage
            MailMessage mailMessage = new MailMessage(senderEmail, adminEmail, subject, body);

            // Cấu hình đối tượng SmtpClient
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

            // Gửi mail
            smtpClient.Send(mailMessage);
        }

        // Phương thức gửi mail thông báo đặt phòng cho khách hàng
        private void SendBookingConfirmationEmail(Booking booking)
        {
            // Địa chỉ email và mật khẩu của tài khoản gửi mail
            string senderEmail = "phamphongvt1998@gmail.com";
            string senderPassword = "fckmwwkbkjwbvboh";

            // Địa chỉ email của khách hàng nhận mail
            string recipientEmail = booking.email;

            // Tiêu đề email
            string subject = "Xác nhận đơn đặt phòng";

            // Tạo nội dung email
            string body = @"
                 <html xmlns=""http://www.w3.org/1999/xhtml"" xmlns:o=""urn:schemas-microsoft-com:office:office"" style=""width:100%;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;padding:0;Margin:0"">
                 <head>
                  <meta charset=""UTF-8"">
                  <meta content=""width=device-width, initial-scale=1"" name=""viewport"">
                  <meta name=""x-apple-disable-message-reformatting"">
                  <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
                  <meta content=""telephone=no"" name=""format-detection"">
                  <title>Trigger newsletter</title><!--[if (mso 16)]>
                    <style type=""text/css"">
                    a {text-decoration: none;}
                    </style>
                    <![endif]--><!--[if gte mso 9]><style>sup { font-size: 100% !important; }</style><![endif]--><!--[if gte mso 9]>
                <xml>
                    <o:OfficeDocumentSettings>
                    <o:AllowPNG></o:AllowPNG>
                    <o:PixelsPerInch>96</o:PixelsPerInch>
                    </o:OfficeDocumentSettings>
                </xml>
                <![endif]-->
                  <style type=""text/css"">
                #outlook a {
	                padding:0;
                }
                .ExternalClass {
	                width:100%;
                }
                .ExternalClass,
                .ExternalClass p,
                .ExternalClass span,
                .ExternalClass font,
                .ExternalClass td,
                .ExternalClass div {
	                line-height:100%;
                }
                .es-button {
	                mso-style-priority:100!important;
	                text-decoration:none!important;
                }
                a[x-apple-data-detectors] {
	                color:inherit!important;
	                text-decoration:none!important;
	                font-size:inherit!important;
	                font-family:inherit!important;
	                font-weight:inherit!important;
	                line-height:inherit!important;
                }
                .es-desk-hidden {
	                display:none;
	                float:left;
	                overflow:hidden;
	                width:0;
	                max-height:0;
	                line-height:0;
	                mso-hide:all;
                }
                @media only screen and (max-width:600px) {p, ul li, ol li, a { line-height:150%!important } h1, h2, h3, h1 a, h2 a, h3 a { line-height:120%!important } h1 { font-size:30px!important; text-align:center } h2 { font-size:26px!important; text-align:center } h3 { font-size:20px!important; text-align:center } .es-header-body h1 a, .es-content-body h1 a, .es-footer-body h1 a { font-size:30px!important } .es-header-body h2 a, .es-content-body h2 a, .es-footer-body h2 a { font-size:26px!important } .es-header-body h3 a, .es-content-body h3 a, .es-footer-body h3 a { font-size:20px!important } .es-header-body p, .es-header-body ul li, .es-header-body ol li, .es-header-body a { font-size:16px!important } .es-content-body p, .es-content-body ul li, .es-content-body ol li, .es-content-body a { font-size:16px!important } .es-footer-body p, .es-footer-body ul li, .es-footer-body ol li, .es-footer-body a { font-size:16px!important } .es-infoblock p, .es-infoblock ul li, .es-infoblock ol li, .es-infoblock a { font-size:12px!important } *[class=""gmail-fix""] { display:none!important } .es-m-txt-c, .es-m-txt-c h1, .es-m-txt-c h2, .es-m-txt-c h3 { text-align:center!important } .es-m-txt-r, .es-m-txt-r h1, .es-m-txt-r h2, .es-m-txt-r h3 { text-align:right!important } .es-m-txt-l, .es-m-txt-l h1, .es-m-txt-l h2, .es-m-txt-l h3 { text-align:left!important } .es-m-txt-r img, .es-m-txt-c img, .es-m-txt-l img { display:inline!important } .es-button-border { display:block!important } a.es-button, button.es-button { font-size:20px!important; display:block!important; border-left-width:0px!important; border-right-width:0px!important } .es-btn-fw { border-width:10px 0px!important; text-align:center!important } .es-adaptive table, .es-btn-fw, .es-btn-fw-brdr, .es-left, .es-right { width:100%!important } .es-content table, .es-header table, .es-footer table, .es-content, .es-footer, .es-header { width:100%!important; max-width:600px!important } .es-adapt-td { display:block!important; width:100%!important } .adapt-img { width:100%!important; height:auto!important } .es-m-p0 { padding:0px!important } .es-m-p0r { padding-right:0px!important } .es-m-p0l { padding-left:0px!important } .es-m-p0t { padding-top:0px!important } .es-m-p0b { padding-bottom:0!important } .es-m-p20b { padding-bottom:20px!important } .es-mobile-hidden, .es-hidden { display:none!important } tr.es-desk-hidden, td.es-desk-hidden, table.es-desk-hidden { width:auto!important; overflow:visible!important; float:none!important; max-height:inherit!important; line-height:inherit!important } tr.es-desk-hidden { display:table-row!important } table.es-desk-hidden { display:table!important } td.es-desk-menu-hidden { display:table-cell!important } .es-menu td { width:1%!important } table.es-table-not-adapt, .esd-block-html table { width:auto!important } table.es-social { display:inline-block!important } table.es-social td { display:inline-block!important } .es-menu td a { font-size:16px!important } .es-desk-hidden { display:table-row!important; width:auto!important; overflow:visible!important; max-height:inherit!important } }
                </style>
                 </head>
                 <body style=""width:100%;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;font-family:arial, 'helvetica neue', helvetica, sans-serif;padding:0;Margin:0"">
                  <div class=""es-wrapper-color"" style=""background-color:#EFEFEF""><!--[if gte mso 9]>
			                <v:background xmlns:v=""urn:schemas-microsoft-com:vml"" fill=""t"">
				                <v:fill type=""tile"" color=""#efefef""></v:fill>
			                </v:background>
		                <![endif]-->
                   <table class=""es-wrapper"" width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;padding:0;Margin:0;width:100%;height:100%;background-repeat:repeat;background-position:center top;background-color:#EFEFEF"">
                     <tr style=""border-collapse:collapse"">
                      <td valign=""top"" style=""padding:0;Margin:0"">
                       <table cellpadding=""0"" cellspacing=""0"" class=""es-content"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%"">
                         <tr style=""border-collapse:collapse"">
                          <td class=""es-adaptive"" align=""center"" style=""padding:0;Margin:0"">
                           <table class=""es-content-body"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#efefef;width:600px"" cellspacing=""0"" cellpadding=""0"" bgcolor=""#ffffff"" align=""center"">
                             <tr style=""border-collapse:collapse"">
                              <td align=""left"" style=""Margin:0;padding-top:15px;padding-bottom:15px;padding-left:20px;padding-right:20px""><!--[if mso]><table style=""width:560px"" cellpadding=""0"" cellspacing=""0""><tr><td style=""width:270px"" valign=""top""><![endif]-->
                               <table class=""es-left"" cellspacing=""0"" cellpadding=""0"" align=""left"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left"">
                                 <tr style=""border-collapse:collapse"">
                                  <td align=""left"" style=""padding:0;Margin:0;width:270px"">
                                   <table width=""100%"" cellspacing=""0"" cellpadding=""0"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                                     <tr style=""border-collapse:collapse"">
                                      <td class=""es-infoblock es-m-txt-c"" align=""left"" style=""padding:0;Margin:0;line-height:14px;font-size:12px;color:#CCCCCC""><p style=""Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:14px;color:#CCCCCC;font-size:12px"">Wellcome to MEOQ HOTEL</p></td>
                                     </tr>
                                   </table></td>
                                 </tr>
                               </table><!--[if mso]></td><td style=""width:20px""></td><td style=""width:270px"" valign=""top""><![endif]-->
                               <table class=""es-right"" cellspacing=""0"" cellpadding=""0"" align=""right"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:right"">
                                 <tr style=""border-collapse:collapse"">
                                  <td align=""left"" style=""padding:0;Margin:0;width:270px"">
                                   <table width=""100%"" cellspacing=""0"" cellpadding=""0"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                                   </table></td>
                                 </tr>
                               </table><!--[if mso]></td></tr></table><![endif]--></td>
                             </tr>
                           </table></td>
                         </tr>
                       </table>
                       <table cellpadding=""0"" cellspacing=""0"" class=""es-header"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%;background-color:transparent;background-repeat:repeat;background-position:center top"">
                         <tr style=""border-collapse:collapse"">
                          <td align=""center"" style=""padding:0;Margin:0"">
                           <table class=""es-header-body"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#fef5e4;width:600px"" cellspacing=""0"" cellpadding=""0"" bgcolor=""#fef5e4"" align=""center"">
                             <tr style=""border-collapse:collapse"">
                              <td align=""left"" style=""Margin:0;padding-top:5px;padding-bottom:5px;padding-left:15px;padding-right:15px""><!--[if mso]><table style=""width:570px"" cellpadding=""0"" cellspacing=""0""><tr><td style=""width:180px"" valign=""top""><![endif]-->
                               <table class=""es-left"" cellspacing=""0"" cellpadding=""0"" align=""left"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left"">
                                 <tr style=""border-collapse:collapse"">
                                  <td class=""es-m-p0r"" valign=""top"" align=""center"" style=""padding:0;Margin:0;width:180px"">
                                   <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                                     <tr style=""border-collapse:collapse"">
                                      <td align=""center"" style=""padding:0;Margin:0;display:none""></td>
                                     </tr>
                                   </table></td>
                                 </tr>
                               </table><!--[if mso]></td><td style=""width:20px""></td><td style=""width:370px"" valign=""top""><![endif]-->
                               <table cellspacing=""0"" cellpadding=""0"" align=""right"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                                 <tr style=""border-collapse:collapse"">
                                  <td align=""left"" style=""padding:0;Margin:0;width:370px"">
                                   <table width=""100%"" cellspacing=""0"" cellpadding=""0"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                                     <tr style=""border-collapse:collapse"">
                                      <td class=""es-m-p0l es-m-txt-c"" align=""left"" style=""padding:0;Margin:0;padding-left:15px;font-size:0px""><a href=""#"" target=""_blank"" style=""-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;text-decoration:underline;color:#999999;font-size:14px""><img src=""https://mxibzo.stripocdn.email/content/guids/CABINET_6c6b17a54ee1ad51495532e31acd78f8f6872555add75c7c93f7ba25b0bc4f1e/images/logo2.png"" alt=""Petshop logo"" title=""Petshop logo"" width=""118"" style=""display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic""></a></td>
                                     </tr>
                                   </table></td>
                                 </tr>
                               </table><!--[if mso]></td></tr></table><![endif]--></td>
                             </tr>
                           </table></td>
                         </tr>
                       </table>
                       <table class=""es-content"" cellspacing=""0"" cellpadding=""0"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%"">
                         <tr style=""border-collapse:collapse"">
                          <td align=""center"" style=""padding:0;Margin:0"">
                           <table class=""es-content-body"" cellspacing=""0"" cellpadding=""0"" bgcolor=""#ffffff"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#FFFFFF;width:600px"">
                             <tr style=""border-collapse:collapse"">
                              <td align=""left"" style=""Margin:0;padding-top:10px;padding-bottom:10px;padding-left:20px;padding-right:20px"">
                               <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                                 <tr style=""border-collapse:collapse"">
                                  <td valign=""top"" align=""center"" style=""padding:0;Margin:0;width:560px"">
                                   <table style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:separate;border-spacing:0px;border-radius:0px"" width=""100%"" cellspacing=""0"" cellpadding=""0"" role=""presentation"">
                                     <tr style=""border-collapse:collapse"">
                                      <td align=""center"" style=""padding:0;Margin:0;padding-top:10px;padding-bottom:15px""><h1 style=""Margin:0;line-height:36px;mso-line-height-rule:exactly;font-family:'trebuchet ms', helvetica, sans-serif;font-size:30px;font-style:normal;font-weight:normal;color:#333333"">Cảm Ơn Quý Khách</h1></td>
                                     </tr>
                                     <tr style=""border-collapse:collapse"">
                                      <td align=""center"" style=""Margin:0;padding-top:5px;padding-bottom:5px;padding-left:40px;padding-right:40px""><p style=""Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;color:#333333;font-size:14px"">Chúng tôi đã nhận được thông tin</p></td>
                                     </tr>
                                   </table></td>
                                 </tr>
                               </table></td>
                             </tr>
                           </table></td>
                         </tr>
                       </table>
                       <table class=""es-content"" cellspacing=""0"" cellpadding=""0"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%"">
                         <tr style=""border-collapse:collapse"">
                          <td align=""center"" style=""padding:0;Margin:0"">
                           <table class=""es-content-body"" cellspacing=""0"" cellpadding=""0"" bgcolor=""#ffffff"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#FFFFFF;width:600px"">
                             <tr style=""border-collapse:collapse"">
                              <td align=""left"" style=""Margin:0;padding-top:20px;padding-left:20px;padding-right:20px;padding-bottom:30px""><!--[if mso]><table style=""width:560px"" cellpadding=""0"" cellspacing=""0""><tr><td style=""width:280px"" valign=""top""><![endif]-->
                               <table class=""es-left"" cellspacing=""0"" cellpadding=""0"" align=""left"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left"">
                                 <tr style=""border-collapse:collapse"">
                                  <td class=""es-m-p20b"" align=""left"" style=""padding:0;Margin:0;width:280px"">
                                   <table style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:separate;border-spacing:0px;background-color:#fef9ef;border-color:#efefef;border-width:1px 0px 1px 1px;border-style:solid"" width=""100%"" cellspacing=""0"" cellpadding=""0"" bgcolor=""#fef9ef"" role=""presentation"">
                                     <tr style=""border-collapse:collapse"">
                                      <td align=""left"" style=""Margin:0;padding-bottom:10px;padding-top:15px;padding-left:20px;padding-right:20px""><h4 style=""Margin:0;line-height:120%;mso-line-height-rule:exactly;font-family:'trebuchet ms', helvetica, sans-serif"">Thông tin đặt phòng:</h4></td>
                                     </tr>
                                     <tr style=""border-collapse:collapse"">
                                      <td align=""left"" style=""Margin:0;padding-bottom:20px;padding-left:20px;padding-right:20px;padding-top:40px"">
                                       <table style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:100%"" class=""cke_show_border"" cellspacing=""1"" cellpadding=""1"" border=""0"" align=""left"" role=""presentation"">
                                         <tr style=""border-collapse:collapse"">
                                          <td style=""padding:0;Margin:0;font-size:14px;line-height:21px"">Mã đặt phòng:</td>
                                          <td style=""padding:0;Margin:0;font-size:14px;line-height:21px"">"+ booking.Room.roomid + @"</td>
                                         </tr>
                                         <tr style=""border-collapse:collapse"">
                                          <td style=""padding:0;Margin:0;font-size:14px;line-height:21px"">Ngày đặt phòng:</td>
                                          <td style=""padding:0;Margin:0;font-size:14px;line-height:21px"">"+ booking.bookingdate + @"</td>
                                         </tr>
                                         <tr style=""border-collapse:collapse"">
                                          <td style=""padding:0;Margin:0;font-size:14px;line-height:21px"">Tổng tiền:</td>
                                          <td style=""padding:0;Margin:0;font-size:14px;line-height:21px"">"+ @String.Format("{0:0,0}", booking.total)+"VND"+ @"</td>
                                         </tr>
                                       </table><p style=""Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;color:#333333;font-size:14px""><br></p></td>
                                     </tr>
                                   </table></td>
                                 </tr>
                               </table><!--[if mso]></td><td style=""width:0px""></td><td style=""width:280px"" valign=""top""><![endif]-->
                               <table class=""es-right"" cellspacing=""0"" cellpadding=""0"" align=""right"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:right"">
                                 <tr style=""border-collapse:collapse"">
                                  <td align=""left"" style=""padding:0;Margin:0;width:280px"">
                                   <table style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:separate;border-spacing:0px;background-color:#fef9ef;border-width:1px;border-style:solid;border-color:#efefef"" width=""100%"" cellspacing=""0"" cellpadding=""0"" bgcolor=""#fef9ef"" role=""presentation"">
                                     <tr style=""border-collapse:collapse"">
                                      <td align=""left"" style=""Margin:0;padding-bottom:10px;padding-top:20px;padding-left:20px;padding-right:20px""><h4 style=""Margin:0;line-height:120%;mso-line-height-rule:exactly;font-family:'trebuchet ms', helvetica, sans-serif"">Thông tin nhận phòng:</h4></td>
                                     </tr>
                                     <tr style=""border-collapse:collapse"">
                                      <td align=""left"" style=""padding:0;Margin:0;padding-bottom:15px;padding-left:20px;padding-right:20px""><p style=""Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;color:#333333;font-size:14px"">Tên Khách Hàng: "+ booking.customername +@"&nbsp;</p><p style=""Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;color:#333333;font-size:14px"">Số điện thoại:"+ booking.phone +@"&nbsp;</p><p style=""Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;color:#333333;font-size:14px"">Email:"+ booking.email + @"&nbsp;</p><p style=""Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;color:#333333;font-size:14px"">Ngày nhận phòng:"+ booking.checkin + @"</p><p style=""Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;color:#333333;font-size:14px"">Ngày trả phòng:"+ booking.checkout + @"&nbsp;</p></td>
                                     </tr>
                                   </table></td>
                                 </tr>
                               </table><!--[if mso]></td></tr></table><![endif]--></td>
                             </tr>
                           </table></td>
                         </tr>
                       </table>
                       <table class=""es-content"" cellspacing=""0"" cellpadding=""0"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%"">
                         <tr style=""border-collapse:collapse"">
                          <td align=""center"" style=""padding:0;Margin:0"">
                           <table class=""es-content-body"" cellspacing=""0"" cellpadding=""0"" bgcolor=""#ffffff"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#FFFFFF;width:600px"">
                             <tr style=""border-collapse:collapse"">
                              <td align=""left"" style=""Margin:0;padding-top:10px;padding-bottom:10px;padding-left:20px;padding-right:20px""><!--[if mso]><table style=""width:560px"" cellpadding=""0"" cellspacing=""0""><tr><td style=""width:270px"" valign=""top""><![endif]-->
                               <table class=""es-left"" cellspacing=""0"" cellpadding=""0"" align=""left"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left"">
                                 <tr style=""border-collapse:collapse"">
                                  <td class=""es-m-p0r es-m-p20b"" valign=""top"" align=""center"" style=""padding:0;Margin:0;width:270px"">
                                   <table width=""100%"" cellspacing=""0"" cellpadding=""0"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                                     <tr style=""border-collapse:collapse"">
                                      <td align=""left"" style=""padding:0;Margin:0;padding-left:20px""><h4 style=""Margin:0;line-height:120%;mso-line-height-rule:exactly;font-family:'trebuchet ms', helvetica, sans-serif"">Thông Tin Phòng</h4></td>
                                     </tr>
                                   </table></td>
                                 </tr>
                               </table><!--[if mso]></td><td style=""width:20px""></td><td style=""width:270px"" valign=""top""><![endif]-->  
                             </tr>
                             <tr style=""border-collapse:collapse"">
                              <td align=""left"" style=""padding:0;Margin:0;padding-left:20px;padding-right:20px"">
                               <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                                 <tr style=""border-collapse:collapse"">
                                  <td valign=""top"" align=""center"" style=""padding:0;Margin:0;width:560px"">
                                   <table width=""100%"" cellspacing=""0"" cellpadding=""0"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                                     <tr style=""border-collapse:collapse"">
                                      <td align=""center"" style=""padding:0;Margin:0;padding-bottom:10px;font-size:0"">
                                       <table width=""100%"" height=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                                         <tr style=""border-collapse:collapse"">
                                          <td style=""padding:0;Margin:0;border-bottom:1px solid #efefef;background:#FFFFFF none repeat scroll 0% 0%;height:1px;width:100%;margin:0px""></td>
                                         </tr>
                                       </table></td>
                                     </tr>
                                   </table></td>
                                 </tr>
                               </table></td>
                             </tr>
                             <tr style=""border-collapse:collapse"">
                              <td align=""left"" style=""Margin:0;padding-top:5px;padding-bottom:10px;padding-left:20px;padding-right:20px""><!--[if mso]><table style=""width:560px"" cellpadding=""0"" cellspacing=""0""><tr><td style=""width:178px"" valign=""top""><![endif]-->
                               <table class=""es-left"" cellspacing=""0"" cellpadding=""0"" align=""left"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left"">
                                 <tr style=""border-collapse:collapse"">
                                  <td class=""es-m-p0r es-m-p20b"" valign=""top"" align=""center"" style=""padding:0;Margin:0;width:178px"">
                                   <table width=""100%"" cellspacing=""0"" cellpadding=""0"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                                     <tr style=""border-collapse:collapse"">
                                      <td align=""center"" style=""padding:0;Margin:0;font-size:0""><a href=""#"" target=""_blank"" style=""-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;text-decoration:underline;color:#D48344;font-size:14px""><img src="" alt="""" class=""adapt-img"" title="""" width=""125"" style=""display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic""></a></td>
                                     </tr>
                                   </table></td>
                                 </tr>
                               </table><!--[if mso]></td><td style=""width:20px""></td><td style=""width:362px"" valign=""top""><![endif]-->
                               <table cellspacing=""0"" cellpadding=""0"" align=""right"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                                 <tr style=""border-collapse:collapse"">
                                  <td align=""left"" style=""padding:0;Margin:0;width:362px"">
                                   <table width=""100%"" cellspacing=""0"" cellpadding=""0"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                                     <tr style=""border-collapse:collapse"">
                                      <td align=""left"" style=""padding:0;Margin:0""><p style=""Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;color:#333333;font-size:14px""><br></p>
                                       <table style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:100%"" class=""cke_show_border"" cellspacing=""1"" cellpadding=""1"" border=""0"" role=""presentation"">
                                         <tr style=""border-collapse:collapse"">
                                          <td style=""padding:0;Margin:0"">" + booking.Room.roomname + @"</td>
                                          <td style=""padding:0;Margin:0;width:60px;text-align:center"">"+ booking.numberpeople + @"</td>
                                          <td style=""padding:0;Margin:0;width:100px;text-align:center"">"+ @String.Format("{0:0,0}", booking.Room.price)+"VND"+@"</td>
                                         </tr>
                                       </table><p style=""Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;color:#333333;font-size:14px""><br></p></td>
                                     </tr>
                                   </table></td>
                                 </tr>
                               </table><!--[if mso]></td></tr></table><![endif]--></td>
                             </tr>
                             <tr style=""border-collapse:collapse"">
                              <td align=""left"" style=""padding:0;Margin:0;padding-left:20px;padding-right:20px"">
                               <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                                 <tr style=""border-collapse:collapse"">
                                  <td valign=""top"" align=""center"" style=""padding:0;Margin:0;width:560px"">
                                   <table width=""100%"" cellspacing=""0"" cellpadding=""0"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                                     <tr style=""border-collapse:collapse"">
                                      <td align=""center"" style=""padding:0;Margin:0;padding-bottom:10px;font-size:0"">
                                       <table width=""100%"" height=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                                         <tr style=""border-collapse:collapse"">
                                          <td style=""padding:0;Margin:0;border-bottom:1px solid #efefef;background:#FFFFFF none repeat scroll 0% 0%;height:1px;width:100%;margin:0px""></td>
                                         </tr>
                                       </table></td>
                                     </tr>
                                   </table></td>
                                 </tr>
                               </table></td>
                             </tr>
                             <tr style=""border-collapse:collapse"">
                              <td align=""left"" style=""padding:0;Margin:0;padding-left:20px;padding-right:20px"">
                               <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                                 <tr style=""border-collapse:collapse"">
                                  <td valign=""top"" align=""center"" style=""padding:0;Margin:0;width:560px"">
                                   <table width=""100%"" cellspacing=""0"" cellpadding=""0"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                                     <tr style=""border-collapse:collapse"">
                                      <td align=""center"" style=""padding:0;Margin:0;padding-bottom:10px;font-size:0"">
                                       <table width=""100%"" height=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                                         <tr style=""border-collapse:collapse"">
                                          <td style=""padding:0;Margin:0;border-bottom:1px solid #efefef;background:#FFFFFF none repeat scroll 0% 0%;height:1px;width:100%;margin:0px""></td>
                                         </tr>
                                       </table></td>
                                     </tr>
                                   </table></td>
                                 </tr>
                               </table></td>
                             </tr>
                             <tr style=""border-collapse:collapse"">
                              <td align=""left"" style=""padding:0;Margin:0;padding-left:20px;padding-right:20px"">
                               <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                                 <tr style=""border-collapse:collapse"">
                                  <td valign=""top"" align=""center"" style=""padding:0;Margin:0;width:560px"">
                                   <table width=""100%"" cellspacing=""0"" cellpadding=""0"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                                     <tr style=""border-collapse:collapse"">
                                      <td align=""center"" style=""padding:0;Margin:0;padding-bottom:10px;font-size:0"">
                                       <table width=""100%"" height=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                                         <tr style=""border-collapse:collapse"">
                                          <td style=""padding:0;Margin:0;border-bottom:1px solid #efefef;background:#FFFFFF none repeat scroll 0% 0%;height:1px;width:100%;margin:0px""></td>
                                         </tr>
                                       </table></td>
                                     </tr>
                                   </table></td>
                                 </tr>
                               </table></td>
                             </tr>
                           </table></td>
                         </tr>
                       </table>
                       <table cellpadding=""0"" cellspacing=""0"" class=""es-footer"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%;background-color:transparent;background-repeat:repeat;background-position:center top"">
                         <tr style=""border-collapse:collapse"">
                          <td align=""center"" style=""padding:0;Margin:0"">
                           <table class=""es-footer-body"" cellspacing=""0"" cellpadding=""0"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#FEF5E4;width:600px"">
                             <tr style=""border-collapse:collapse"">
                              <td align=""left"" style=""padding:20px;Margin:0""><!--[if mso]><table style=""width:560px"" cellpadding=""0"" cellspacing=""0""><tr><td style=""width:178px"" valign=""top""><![endif]-->
                               <table class=""es-left"" cellspacing=""0"" cellpadding=""0"" align=""left"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left"">
                                 <tr style=""border-collapse:collapse"">
                                  <td class=""es-m-p0r es-m-p20b"" valign=""top"" align=""center"" style=""padding:0;Margin:0;width:178px"">
                                   <table width=""100%"" cellspacing=""0"" cellpadding=""0"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                                     <tr style=""border-collapse:collapse"">
                                      <td class=""es-m-p0l es-m-txt-c"" align=""left"" style=""padding:0;Margin:0;font-size:0px""><img src=""https://mxibzo.stripocdn.email/content/guids/CABINET_6c6b17a54ee1ad51495532e31acd78f8f6872555add75c7c93f7ba25b0bc4f1e/images/logo2.png"" alt=""Petshop logo"" title=""Petshop logo"" width=""108"" style=""display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic""></td>
                                     </tr>
                                     <tr style=""border-collapse:collapse"">
                                      <td class=""es-m-txt-c"" align=""left"" style=""padding:0;Margin:0;padding-bottom:5px;padding-top:10px""><p style=""Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;color:#333333;font-size:14px"">17/1a Đường số 7 Tam Bình ,Thủ Đức, Tp Hồ Chí Minh</p></td>
                                     </tr>
                                   </table></td>
                                 </tr>
                               </table><!--[if mso]></td><td style=""width:20px""></td><td style=""width:362px"" valign=""top""><![endif]-->
                               <table cellspacing=""0"" cellpadding=""0"" align=""right"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                                 <tr style=""border-collapse:collapse"">
                                  <td align=""left"" style=""padding:0;Margin:0;width:362px"">
                                   <table width=""100%"" cellspacing=""0"" cellpadding=""0"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                                     <tr style=""border-collapse:collapse"">
                                      <td class=""es-m-txt-c"" align=""center"" style=""padding:0;Margin:0;padding-top:15px;padding-bottom:20px""><p style=""Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:30px;color:#333333;font-size:20px"">Thông Tin Liên Hệ</p></td>
                                     </tr>
                                     <tr style=""border-collapse:collapse"">
                                      <td class=""es-m-txt-c"" align=""center"" style=""padding:0;Margin:0""><p style=""Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;color:#333333;font-size:14px"">MEOQ HOTEL&nbsp;</p><p style=""Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;color:#333333;font-size:14px""><br></p><p style=""Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;color:#333333;font-size:14px"">Số Điện Thoại: 0924072002</p><p style=""Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;color:#333333;font-size:14px"">Email: meoq.hotel@gmail.com</p></td>
                                     </tr>
                                   </table></td>
                                 </tr>
                               </table><!--[if mso]></td></tr></table><![endif]--></td>
                             </tr>
                           </table></td>
                         </tr>
                       </table>
                       <table class=""es-content"" cellspacing=""0"" cellpadding=""0"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%"">
                         <tr style=""border-collapse:collapse"">
                          <td align=""center"" style=""padding:0;Margin:0"">
                           <table class=""es-content-body"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:600px"" cellspacing=""0"" cellpadding=""0"" align=""center"">
                             <tr style=""border-collapse:collapse"">
                              <td align=""left"" style=""Margin:0;padding-left:20px;padding-right:20px;padding-top:30px;padding-bottom:30px"">
                               <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                                 <tr style=""border-collapse:collapse"">
                                  <td valign=""top"" align=""center"" style=""padding:0;Margin:0;width:560px"">
                                   <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                                     <tr style=""border-collapse:collapse"">
                                      <td align=""center"" style=""padding:0;Margin:0;display:none""></td>
                                     </tr>
                                   </table></td>
                                 </tr>
                               </table></td>
                             </tr>
                           </table></td>
                         </tr>
                       </table></td>
                     </tr>
                   </table>
                  </div>
                 </body>
                </html>
            ";

            // Tạo đối tượng MailMessage
            MailMessage mailMessage = new MailMessage(senderEmail, recipientEmail, subject, body);
            mailMessage.IsBodyHtml = true;

            // Cấu hình đối tượng SmtpClient
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

            // Gửi mail
            smtpClient.Send(mailMessage);
        }


        
        //public ActionResult HuyDatPhong(int id)
        //{
        //    // Tìm đơn đặt phòng dựa trên ID
        //    var booking = db.Bookings.Find(id);

        //    if (booking == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    // Kiểm tra trạng thái hiện tại của đơn đặt phòng
        //    if (booking.statusid != 1) // Kiểm tra trạng thái đã được xác nhận
        //    {
        //        ViewBag.ErrorMessage = "Không thể hủy đặt phòng. Đơn đặt phòng không ở trạng thái chờ xác nhận.";
        //        return View("Error"); // Hiển thị trang lỗi hoặc chuyển hướng đến trang khác tùy theo yêu cầu của bạn
        //    }

        //    // Cập nhật trạng thái thành 5 (hủy đặt phòng)
        //    booking.statusid = 5;

        //    // Lưu thay đổi vào cơ sở dữ liệu
        //    db.SaveChanges();

        //    // Chuyển hướng đến trang thông báo hủy đặt phòng thành công
        //    return RedirectToAction("HuyDatPhongSuccess");
        //}
        //public ActionResult HuyDatPhongSuccess()
        //{
        //    return View();
        //}


    }

}
