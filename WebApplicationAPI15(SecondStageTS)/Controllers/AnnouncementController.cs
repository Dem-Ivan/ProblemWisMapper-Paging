using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationAPI15_SecondStageTS_.dto;
using WebApplicationAPI15_SecondStageTS_.Models;
using WebApplicationAPI15_SecondStageTS_.Services;

namespace WebApplicationAPI15_SecondStageTS_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        ApplicationContext db;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _appEnwironment;
        private readonly IRecaptchaService _recaptcha;

        public AnnouncementController(ApplicationContext context, IMapper mapper, IWebHostEnvironment appEnwironment, IRecaptchaService recaptcha)
        {
            db = context;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _appEnwironment = appEnwironment;
            _recaptcha = recaptcha;

            if (!db.Announcements.Any())
            {
                db.Announcements.Add(
                    new Announcement
                    {
                        OrderNumber = 1,
                        user = new User { Name = "Tom" },
                        Text = "пойду добровольно в армию!",
                        CreationDate = DateTime.Now,
                        Rating = 3
                    });
                db.SaveChanges();
            }
        }


        //GET api/announcements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnnouncementDTO>>> Get()
        {
            try {
               // PagedResult<Announcement> dataPage = db.Announcements.GetPaged(1, 5);
               // return new ObjectResult(dataPage);
                var dataPageDTO = await db.Announcements.GetPaged(1, 5);
                return dataPageDTO;

                ////AnnouncementDTO announcementsDTO = _mapper.Map<PagedResult<Announcement>, AnnouncementDTO>(dataPage);
                ////return new ObjectResult(announcementsDTO);

            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message, e.StackTrace });
            }
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<AnnouncementDTO>>> Get()
        //{
        //    // return await db.Announcements.ToListAsync();
        //    var query = db.Announcements;
        //    var announcementsDTO = await _mapper.ProjectTo<AnnouncementDTO>(query).ToListAsync();
        //    return announcementsDTO;
        //}

        //[Route("GetImage/{imageName}")]
        [HttpGet("{ImageName}/{id}")]
        public ActionResult GetImage(string ImageName, int id)// мметод выдает изображение по ссылке из бд
        {
            string file_path = Path.Combine(_appEnwironment.ContentRootPath, "MyImages/"+ ImageName);           
           // string file_path = Path.Combine(_appEnwironment.ContentRootPath, ImageName);//"MyImages/Image_1.jpg"
            string file_type = "application/png"; 

            return PhysicalFile(file_path, file_type, ImageName);            
        }

        //GET api/announcement/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AnnouncementDTO>> Get(int id)
        {
            var query = db.Announcements.Where(an => an.Id == id);
            var announcementDTO = await _mapper.ProjectTo<AnnouncementDTO>(query).FirstOrDefaultAsync();

            if (announcementDTO == null)
            {
                return NotFound();
            }

            return Ok(announcementDTO);
        }

        //POST api/announcement
        [HttpPost]
        public async Task<ActionResult<Announcement>> Post(AnnouncementDTO announcementDTO)
        {
            try
            {   
                //раскоментировать для проверки капчи, при наличии годного клиента
                // var captchaResponse = await _recaptcha.Validate(Request.Form);
                //if (!captchaResponse.Success)
                //{
                //    ModelState.AddModelError("reCaptchaError", "reCAPTCHA error occured. Please try again.");
                //    return RedirectToAction(nameof(Index));
                //}
                if (announcementDTO == null)
                {
                    return BadRequest();
                }

                Announcement announcement = _mapper.Map<AnnouncementDTO, Announcement>(announcementDTO);
                announcement.OrderNumber = db.Announcements.Max(e => e.OrderNumber) + 1;
                announcement.CreationDate = DateTime.Now;
                db.Announcements.Add(announcement);
                await db.SaveChangesAsync();

                var query = db.Announcements.Where(an => an.Id == announcement.Id);
                var announcementDTO2 = await _mapper.ProjectTo<AnnouncementDTO>(query).FirstOrDefaultAsync();

                return Ok(announcementDTO2);
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message, e.StackTrace });
            }
        }

        //PUT api/announcement
        [HttpPut]
        public async Task<ActionResult<Announcement>> Put(AnnouncementDTO announcementDTO)
        {
            if (announcementDTO == null)
            {
                return BadRequest();
            }

            Announcement announcement = _mapper.Map<AnnouncementDTO, Announcement>(announcementDTO);

            if (!db.Announcements.Any(an => an.Id == announcement.Id))
            {
                return NotFound();
            }

            db.Update(announcement);
            await db.SaveChangesAsync();

            var query = db.Announcements.Where(an => an.Id == announcement.Id);
            var announcementDTO2 = await _mapper.ProjectTo<AnnouncementDTO>(query).FirstOrDefaultAsync();

            return Ok(announcementDTO2);
        }

        //DELETE api/announcement/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AnnouncementDTO>> DeleteAnnouncement(int id)
        {
            Announcement announcement = db.Announcements.Include(an => an.user).FirstOrDefault(an => an.Id == id);

            //var query = db.Announcements.Where(an => an.Id == id);
            //var announcement = await _mapper.ProjectTo<AnnouncementDTO>(query).FirstOrDefaultAsync();

            if (announcement == null)
            {
                return NotFound();
            }

            db.Announcements.Remove(announcement);
            await db.SaveChangesAsync();
            return Ok(announcement);
        }

        //DELETE api/user/5
        //[HttpDelete]
        //public async Task<ActionResult<User>> DeleteUser(int id)
        //{
        //    User user = db.Users.FirstOrDefault(u => u.Id == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Users.Remove(user);
        //    await db.SaveChangesAsync();
        //    return Ok(user);
        //}
    }
}