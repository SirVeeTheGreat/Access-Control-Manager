using Access_Control_Manager.Interface;
using Access_Control_Manager.Models;
using Microsoft.AspNetCore.Mvc;

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using IronBarCode;
using Color = IronSoftware.Drawing.Color;
using FirebaseAdmin.Auth.Multitenancy;
using System.Net.Mail;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Access_Control_Manager.Controllers
{
    [Authorize(Policy = "RequireAssistantOrManager")]
    public class CheckingInController : Controller
    {
        private readonly IApplicationUser _user;
        private readonly IStudent _student;
        private readonly IDevice _device;
        private readonly IWebHostEnvironment _iWebHostEnvironment;
        

        public CheckingInController(IApplicationUser user, IStudent student, IDevice device, IWebHostEnvironment iWebHostEnvironment)
        {
            _user = user;
            _student = student;
            _device = device;
            _iWebHostEnvironment = iWebHostEnvironment;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Menu()
        {
            return View();
        }

        public IActionResult Start()
        {
            return View();
        }


        public IActionResult CheckingIn()
        {
            return View();
        }

        

        [HttpGet]
        public async Task<IActionResult> AddDevices(long id)
        {
            var student = await _student.GetStudentByStudentNumber(id);
            Device device = new Device
            {
                StudentNumber = student.StudentNumber
            };
            return View("AddDevice", device);
        }




        [HttpGet]
        public IActionResult IdentifyWhoIsCheckingOut()
        {
            return View("Options2");
        }


        [HttpGet]
        public IActionResult GetQrCode()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> CheckOut(string qrcode)
        {
         
            var student = await _student.GetStudentByQrCode(qrcode);

            if (student == null)
            {
                TempData["Error"] = "No student record found";
                return View("GetQrCode");
            }

            string imageUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/QRCodes/" +
                              qrcode;

            ViewBag.img = imageUrl;

            return View("VerifyCheckOut", student);
        }


        public async Task<IActionResult> Verify(string id)
        {
            await _student.CheckOutStudent(id);
            TempData["Success"] = "Student successfully checked out";
            return View("Menu");
        }

        [HttpPost]
        public async Task<IActionResult> AddDevice(Device device)
        {
            try
            {
                await _device.AddDevice(device);
                ViewBag.Id = device.StudentNumber;
                TempData["Success"] = "Device Successfully Added";
                ViewBag.Id = device.StudentNumber;
                ViewBag.DisableBtn = null;
                return View("Options");
            }
            catch
            {
                TempData["Error"] = "Error, Something went wrong";
                return View();
            }

        }


        [HttpGet]
        [Route($"Student")]
        public IActionResult AddStudent()
        {

            return View();

        }

        [HttpPost]
        [Route($"Student")]
        public async Task<IActionResult> AddStudent(Student student)
        {
            string email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _user.GetUserByEmail(email);
            var studentNumber = student.StudentNumber.ToString();

            if (studentNumber.Length != 9)
            {
                TempData["Error"] = $"Invalid student number";
                return View();
            }


            var flag = await _student.CheckIfStudentExists(student.StudentNumber);
            if (flag)
            {
                var exisitingStudent = await _student.GetStudentByStudentNumber(student.StudentNumber);

                if (!exisitingStudent.CheckedOut)
                {
                    TempData["Error"] = "Cannot perform operation, Student has not checkout of campus";
                    return View();
                }

               if (student.HasDevice == true)
               {
                   Device device = new Device
                   {
                       StudentNumber = student.StudentNumber
                   };
                   exisitingStudent.UniqueGeneratedCode = "" + new Random().Next(100000) + new Random().Next(200);
                   ViewBag.Id = student.StudentNumber;
                    await _student.Save();
                    TempData["Success"] = "Student successfully identified";
                    return View("AddDevice", device);
               }
               else
               {
                   exisitingStudent.UniqueGeneratedCode = "" + new Random().Next(100000) +  new Random().Next(200);
                   await _student.Save();
                   ViewBag.Id = student.StudentNumber;
                   ViewBag.DisableBtn = 1;
                   TempData["Success"] = "Student successfully identified";
                    return View("Options");
               }
            }
            else
            {
                student.UniqueGeneratedCode = "" + new Random().Next(100000) + new Random().Next(200);
                student.DateRegistered = DateTime.Now;
                ViewBag.Id = student.StudentNumber;
                student.Campus = user.Campus.Name;
                await _student.AddStudent(student);
                TempData["Success"] = "No existing record, Student successfully added";
                if (student.HasDevice == true)
                {
                    Device device = new Device
                    {
                        StudentNumber = student.StudentNumber
                    };
                    return View("AddDevice", device);

                }
                else
                {
                    student.UniqueGeneratedCode = "" + new Random().Next(100000) + new Random().Next(200);
                    await _student.Save();
              
                    ViewBag.DisableBtn = 1;
                    ViewBag.Id = student.StudentNumber;
                    TempData["Success"] = "Student successfully identified";
                    return View("Options") ;
                }
            }
            TempData["Success"] = "Error";
            return View();
        }

   
        public async Task<IActionResult> CompleteCheck(int id)
        {
            
            await _student.CheckInStudent(id);
            var student = await _student.GetStudentSummary(id);
            string hasDevice = "No";

          

            if (student.HasDevice == true)
            {
                hasDevice = "Yes";
            }

            string imageUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/QRCodes/" +
                              student;


            var htmlcontent = $@"
                                        <html>
                                        <head>
                                               <style>
                                                body {{                                            
                                                    padding: 20px;
                                                    font-family: 'Lato', Arial, sans-serif;
                                                    line-height: 1.6;
                                                }}
                                                table {{
                                                    border-collapse: collapse;
                                                    width: 100%;
                                                    max-width: 500px;
                                                }}
                                                td {{
                                                    border: 1px solid #ddd;
                                                    padding: 8px;
                                                }}
                                                td:first-child {{
                                                    font-weight: bold;
                                                }}
                                            </style>
                                        </head>
                                        <body>
                                            <div style='text-align: center;'>
                                               <h1>Student Access Control Auto-Generated Slip</1>
                                            </div>
                                            <h3>Dear Student</p>
                                            <br/>
                                            <h3>You are receiving this email because you've just checked-in at Central University of Technology {student.Campus} campus</p>
                                            <br/>
                                            <h5>Your Check-in details, kindly keep it safe</h5>
                                            <br/>
                                            <table>
                                                <tr>
                                                    <td>Student number</td>
                                                    <td>{student.StudentNumber}</td>
                                                </tr>
                                                <tr>
                                                    <td>Check-in Date </td>
                                                    <td>{student.CheckIn}</td>
                                                </tr>
                                                <tr>
                                                    <td>Has a device</td>
                                                    <td>{hasDevice}</td>
                                                </tr>
                                                
                                                <tr>
                                                    <td><i class=""fi fi-sr-hand-holding-usd""></i>Your Unique Check-Out Code</td>
                                                    <td>{student.UniqueGeneratedCode}</td>
                                                </tr>
                                                
                                                          
                                            </table>

                                            <h4>Check-Out Instructions:</h4>
                                            <p>Kindly present your QR Code or Unique Check-Out code to the protection officer to verify that you are carrying the same devices that you registered</p>
                                            <br/>
                                            
                                          
                                            
                          
                                        </body>
                                        </html>
                                    ";

            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("support@therentalguy.co.za", "Central University of Technology");
            var subject = "CUT ACCESS SLIP";
            var to = new EmailAddress($"v.mabuya888@gmail.com", "CUT Student");
            var htmlContent = htmlcontent;
            var plainTextContent = "";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.Body.ReadAsStringAsync().Result);
            TempData["Success"] = "Check-in Successful, an email has been sent to their student mail";
            return View("Start");
        }


        public async Task<IActionResult> Summary(long id)
        {
            var studentSummary = await _student.GetStudentSummary(id);
             await GenerateQrCode(id);
            return View("Summary", studentSummary);
        }

       
        public async Task GenerateQrCode(long id)
        {
            try
            {
                var student = await _student.GetStudentByStudentNumber(id);

                GeneratedBarcode barcode = QRCodeWriter.CreateQrCode(student.UniqueGeneratedCode, 200);
                barcode.AddBarcodeValueTextAboveBarcode();
                barcode.SetMargins(10);
                barcode.ChangeBackgroundColor(Color.Cornsilk);

                string path = Path.Combine(_iWebHostEnvironment.WebRootPath, "QRCodes");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string filePath = Path.Combine(_iWebHostEnvironment.WebRootPath,
                    $"QrCodes/{student.UniqueGeneratedCode}.png");

                barcode.SaveAsPng(filePath);
                string filename = Path.GetFileName(filePath);

                string imageUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/QRCodes/" +
                                 filename ;

                student.QRCodePath = imageUrl;
                await _student.Save();

                ViewBag.qrcode = imageUrl;

            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Something went wrong {ex.ToString()}";
            }

        }

       

       






    }
}
