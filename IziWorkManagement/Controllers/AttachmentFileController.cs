using AutoMapper;
using IziWork.Business.Args;
using IziWork.Business.DTO;
using IziWork.Business.Handlers;
using IziWork.Business.Interfaces;
using IziWorkManagement.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace IziWorkManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentFileController : ControllerBase
    {
        #region Variables
        private readonly AppSettingModel _appSettingModel;
        private readonly ICategoryDetailBusiness _categoryDetailBusiness;
        private readonly ILogger<AttachmentFileController> _logger;
        private readonly IAttachmentFileBusiness _attachmentFileBusiness;
        private readonly IMapper _mapper;
        private string _uploadedFilesFolder = null;
        #endregion
        public AttachmentFileController(
                    ICategoryDetailBusiness categoryDetailBusiness,
                    IOptionsMonitor<AppSettingModel> optionsMonitor,
                    ILogger<AttachmentFileController> logger,
                    IWebHostEnvironment env,
                    IAttachmentFileBusiness attachmentFileBusiness,
                    IMapper mapper
                    )
        {
            _appSettingModel = optionsMonitor.CurrentValue;
            _categoryDetailBusiness = categoryDetailBusiness;
            _logger = logger;
            _uploadedFilesFolder = Path.Combine(env.ContentRootPath, "Attachments");
            _attachmentFileBusiness = attachmentFileBusiness;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("UploadFiles")]
        public async Task<IActionResult> UploadFiles()
        {
            var result = new ResultDTO();
            try
            {
                _logger.LogInformation("Root Attachment File: " + _uploadedFilesFolder);
                // Insert code to save uploaded files to the _uploadedFilesFolder
                var files = Request.Form.Files;
                var fileResults = new List<AttachmentFileDTO>();
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        string uploadingFileName = fileName;
                        string originalFileName = String.Concat(_uploadedFilesFolder, "\\" + (fileName).Trim(new Char[] { '"' }));
                        var fileUniqueName = $"{Path.GetFileNameWithoutExtension(originalFileName)}_{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
                        var filePath = Path.Combine(_uploadedFilesFolder, fileUniqueName);

                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                        //System.IO.File.Move(uploadingFileName, filePath);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        var fileResult = await _attachmentFileBusiness.Save(new AttachmentFileDTO
                        {
                            FileName = $"{Path.GetFileNameWithoutExtension(originalFileName)}{Path.GetExtension(originalFileName)}",
                            Extension = Path.GetExtension(originalFileName),
                            FileDisplayName = $"{Path.GetFileNameWithoutExtension(originalFileName)}{Path.GetExtension(originalFileName)}",
                            FileUniqueName = fileUniqueName,
                            Size = (new System.IO.FileInfo(filePath).Length) / 1024,
                            Type = file.ContentType.ToString(),
                        });
                        if (fileResult != null && fileResult.Object != null && fileResult.Object is AttachmentFileDTO)
                        {
                            fileResults.Add(fileResult.Object as AttachmentFileDTO);
                        }
                    }
                }
                result.Object = fileResults;
            }
            catch (Exception ex)
            {
                result.Object = false;
                result.Messages.Add(ex.Message);
                result.ErrorCodes.Add(0);
                _logger.LogError(ex.Message);
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAttachmentById")]
        public async Task<IActionResult> GetAttachmentById(Guid Id, bool getBase64Image = false)
        {
            var res = new ResultDTO();
            try
            {
                if (Id == Guid.Empty)
                {
                    res.Messages.Add("File Id is invalid");
                }

                var attachment = await _attachmentFileBusiness.Get(Id);
                if (attachment == null)
                {
                    res.Messages.Add("File Id is invalid");
                }

                var att = _mapper.Map<AttachmentFileDTO>(attachment);

                if (getBase64Image)
                {
                    try
                    {
                        Directory.CreateDirectory(_uploadedFilesFolder); // Make sure the folder exists
                        var filepath = Path.Combine(_uploadedFilesFolder, attachment.FileUniqueName);
                        byte[] imageArray = System.IO.File.ReadAllBytes(filepath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                    }
                }

                res.Object = att;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                res.Messages.Add(ex.Message);
                return Ok(res);
            }
            return Ok(res);
        }

        [HttpGet]
        [Route("DeleteAttachmentById")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _attachmentFileBusiness.Delete(id));
        }

        [HttpPost]
        [Route("DeleteMultiFile")]
        public async Task<IActionResult> DeleteMultiFile(List<Guid> Ids)
        {
            try
            {
                return Ok(await _attachmentFileBusiness.DeleteMultiFile(Ids));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error at: DeleteMultiFile", ex.Message);
                return BadRequest("System Error");
            }
        }

        [HttpGet]
        [Route("DownloadAttachmentById")]
        public async Task<IActionResult> DownloadAttachmentById(Guid id)
        {
            var resultDto = new ResultDTO();
            byte[] fileBytes = null;
            try
            {
                if (id == Guid.Empty)
                {
                    resultDto.Messages.Add("Guid not empty");
                    return Ok(resultDto);
                }

                var attachment = await _attachmentFileBusiness.Get(id);
                if (attachment == null)
                {
                    resultDto.Messages.Add("File Id not found");
                    return Ok(resultDto);
                }
                //Tìm kiếm file tại sharepoint List trước không có thì vòng quay lại Attachment
                /*try
                {
                    var _linkdowload = "/Shared Documents";
                    string linkFile = _linkdowload + "/" + attachment.Id + "/" + attachment.FileUniqueName;
                    fileBytes = this.GetDataFromUrl(linkFile);
                }
                catch (Exception ex)
                {
                    fileBytes = null;
                    _logger.LogError(ex, ex.Message);
                }*/
                // nếu vẫn bằng null thì vòng quay lại Attachment down xem có không cho chắc
                if (fileBytes == null)
                {
                    Directory.CreateDirectory(_uploadedFilesFolder); // Make sure the folder exists
                    var filepath = Path.Combine(_uploadedFilesFolder, attachment.FileUniqueName);
                    if (!System.IO.File.Exists(filepath) && attachment.Extension.Equals(".zip", StringComparison.InvariantCultureIgnoreCase))
                    {
                        filepath = Path.Combine(_uploadedFilesFolder, "zip", attachment.FileUniqueName);
                    }

                    if (!System.IO.File.Exists(filepath))
                    {
                        resultDto.Messages.Add("File not found");
                        return Ok(resultDto);
                    }

                    fileBytes = System.IO.File.ReadAllBytes(filepath);
                }

                if (fileBytes != null)
                {
                    var fileResult = new FileResultDTO
                    {
                        FileName = attachment.FileDisplayName,
                        Content = fileBytes,
                        Type = attachment.Type,
                    };
                    resultDto.Object = fileResult;
                    return Ok(resultDto);
                }
                else
                {
                    resultDto.Messages.Add("File Id not found");
                    return Ok(resultDto);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                resultDto.Messages.Add(ex.Message);
                return Ok(resultDto);
            }
        }

        /*public byte[] GetDataFromUrl(string fullUrl)
        {
            byte[] m_Result = null;

            var _SharePointURL = ConfigurationManager.AppSettings["siteUrl"];

            var _Username = ConfigurationManager.AppSettings["SharePoint_Username"];
            var _Domain = ConfigurationManager.AppSettings["SharePoint_Domain"];
            var _Password = ConfigurationManager.AppSettings["SharePoint_Password"];

            using (ClientContext Context = new ClientContext(_SharePointURL))
            {

                Context.AuthenticationMode = ClientAuthenticationMode.FormsAuthentication;
                Context.FormsAuthenticationLoginInfo = new FormsAuthenticationLoginInfo(_Username, _Password);
                Microsoft.SharePoint.Client.File filetoDownload = Context.Web.GetFileByServerRelativeUrl(fullUrl);
                Context.Load(filetoDownload);
                Context.ExecuteQuery();

                var stream = filetoDownload.OpenBinaryStream();
                Context.ExecuteQuery();
                using (var memoryStream = new MemoryStream())
                {
                    stream.Value.CopyTo(memoryStream);
                    m_Result = memoryStream.ToArray();
                }
            }

            return m_Result;
        }*/

        /*public bool CreateFileInDocumentList(string uploadFilePath, string fileUniqueName, AttachmentFileViewModel attachmentFile)
        {
            bool result = false;
            try
            {
                var _SharePointURL = ConfigurationManager.AppSettings["siteUrl"];
                var _Username = ConfigurationManager.AppSettings["SharePoint_Username"];
                var _Domain = ConfigurationManager.AppSettings["SharePoint_Domain"];
                var _Password = ConfigurationManager.AppSettings["SharePoint_Password"];
                using (ClientContext Context = new ClientContext(_SharePointURL))
                {
                    Context.AuthenticationMode = ClientAuthenticationMode.FormsAuthentication;
                    Context.FormsAuthenticationLoginInfo = new FormsAuthenticationLoginInfo(_Username, _Password);

                    var _linkFolder = "/Shared Documents/" + attachmentFile.Id;

                    if (FileExists(Context, _linkFolder)) // trường hợp có rồi thì chỉ việc đưa file lên 
                    {
                        FileCreationInformation fileCreationInformation = new FileCreationInformation
                        {
                            Content = System.IO.File.ReadAllBytes(uploadFilePath),
                            Url = System.IO.Path.Combine(_linkFolder, System.IO.Path.GetFileName(uploadFilePath)),
                            Overwrite = true
                        };

                        var list = Context.Web.Lists.GetByTitle("Documents");
                        var uploadFile = list.RootFolder.Files.Add(fileCreationInformation);
                        Context.Load(uploadFile);
                        Context.ExecuteQuery();

                        result = true;
                    }
                    else // chưa có thì tạo ra folder trước rồi đưa file lên
                    {
                        var list = Context.Web.Lists.GetByTitle("Documents");
                        var folder = list.RootFolder;
                        Context.Load(folder);
                        Context.ExecuteQuery();
                        ListItemCreationInformation newItemInfo = new ListItemCreationInformation();
                        newItemInfo.UnderlyingObjectType = FileSystemObjectType.Folder;
                        newItemInfo.LeafName = attachmentFile.Id.ToString();
                        ListItem newListItem = list.AddItem(newItemInfo);
                        newListItem["Title"] = attachmentFile.Id.ToString();
                        newListItem.Update();
                        Context.ExecuteQuery();

                        FileCreationInformation fileCreationInformation = new FileCreationInformation
                        {
                            Content = System.IO.File.ReadAllBytes(uploadFilePath),
                            Url = System.IO.Path.Combine(_linkFolder, System.IO.Path.GetFileName(uploadFilePath)),
                            Overwrite = true
                        };
                        var uploadFile = list.RootFolder.Files.Add(fileCreationInformation);
                        Context.Load(uploadFile);
                        Context.ExecuteQuery();
                        result = true;

                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }*/

        /*private bool FileExists(ClientContext context, string url)
        {
            var file = context.Web.GetFileByServerRelativeUrl(url);
            context.Load(file, f => f.Exists);
            try
            {
                context.ExecuteQuery();
                if (file.Exists)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }*/
    }
}