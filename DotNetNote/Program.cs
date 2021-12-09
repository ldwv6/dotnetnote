using DotNetNote.Data;
using DotNetNote.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();


//[DNN][1] 게시판 관련 서비스 등록            
//[1] 생성자에 문자열로 연결 문자열 지정
//services.AddSingleton<INoteRepository>(
//  new NoteRepository(
//      Configuration["Data:DefaultConnection:ConnectionString"]));            
//[2] 의존성 주입으로 Configuration 주입
//[a] NoteRepository에서 IConfiguration으로 데이터베이스 연결 문자열 접근
builder.Services.AddTransient<INoteRepository, NoteRepository>();
//[b] CommentRepository의 생성자에 데이터베이스 연결문자열 직접 전송
//services.AddSingleton<INoteCommentRepository>(
//    new NoteCommentRepository(
//        Configuration["ConnectionStrings:DefaultConnection"]));
//[b] 위 코드를 아래 코드로 변경
builder.Services.AddTransient<INoteCommentRepository, NoteCommentRepository>();

builder.Services.AddDbContext<DotNetNoteContext>(options => options.UseSqlServer(connectionString));

// Inquiries 모듈 
builder.Services.AddTransient<IInquiryRepository, InquiryRepository>();
builder.Services.AddTransient<IInquiryCommentRepository, InquiryCommentRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
