# Learn English Backend

## Cấu trúc dự án

Dự án sử dụng cấu trúc Controller → Service → Repository → DbContext.
Tên project vẫn là `learn-english-backend`; các chức năng hiện có được sắp xếp
theo cây thư mục dưới đây.

```text
learn-english-backend/
├── Controllers/
│   └── AuthController.cs
├── Models/
│   ├── Entities/
│   │   ├── User.cs
│   │   └── RefreshToken.cs
│   ├── DTOs/                  # RegisterRequest, LoginRequest, AuthResponse, ...
│   └── Results/               # Kết quả nội bộ của service/helper
├── Data/
│   ├── AppDbContext.cs
│   ├── Configurations/
│   └── Migrations/
├── Repositories/
│   ├── Interfaces/
│   │   └── IRefreshTokenRepository.cs
│   └── RefreshTokenRepository.cs
├── Services/
│   ├── Interfaces/
│   │   ├── IAuthService.cs
│   │   └── IJwtTokenGenerator.cs
│   └── AuthService.cs
├── Helpers/
│   ├── JwtHelper.cs
│   ├── JwtOptions.cs
│   └── *ServiceCollectionExtensions.cs
├── Middlewares/
│   └── ExceptionMiddleware.cs
├── Properties/
├── appsettings.json          # Cấu hình local, được bỏ qua bởi Git
├── Program.cs
└── learn-english-backend.csproj
```

Luồng refresh token: `AuthController -> IAuthService -> AuthService ->
IRefreshTokenRepository -> RefreshTokenRepository -> AppDbContext`.
`AuthService` gọi `IJwtTokenGenerator`, được triển khai bởi `Helpers/JwtHelper`.
Tài khoản và mật khẩu được quản lý qua `UserManager<User>` / `SignInManager<User>`
của ASP.NET Core Identity, dùng cùng scoped `AppDbContext`.

Quy ước trách nhiệm:

- Controller nhận request, gọi service và chuyển kết quả thành HTTP response;
  không truy vấn database hoặc xử lý mật khẩu trực tiếp.
- Service thực hiện nghiệp vụ và điều phối việc lưu dữ liệu; không phụ thuộc
  vào `ControllerBase`, `IActionResult` hay mã trạng thái HTTP.
- Request/response DTO định nghĩa contract với client. Không trả trực tiếp
  entity có password hash, security stamp hoặc refresh-token hash.
- Repository chứa truy vấn EF, tracking và lưu dữ liệu. Rotation lưu việc thu hồi
  token cũ và thêm token mới trong một lần `SaveChangesAsync`; xung đột concurrency
  được chuyển thành kết quả `false` để service xử lý. Lỗi database khác tiếp tục
  được chuyển lên middleware.
- `Models/Results` chứa kết quả nội bộ, không phải response HTTP.
- `Models/Entities` chứa entity; mapping EF nằm ở `Data/Configurations`.
- Các extension trong `Helpers` nối interface với implementation. `Program.cs`
  gọi extension và thiết lập middleware.
- `ExceptionMiddleware` ghi chi tiết lỗi ở server, trả `ProblemDetails` có
  `traceId` cho client và không trả stack trace hay thông tin kết nối.

Đây là phân lớp theo thư mục trong một assembly. Nghiệp vụ học tập sử dụng
`LearningController -> ILearningService -> ILearningRepository -> AppDbContext`
cho bài học, từ vựng và tiến độ đã thuộc theo người dùng. Không cần
`PasswordHasher.cs` riêng: Identity đã cung cấp bộ băm mật khẩu
và kiểm tra mật khẩu qua DI.

## Thêm chức năng mới

1. Tạo request/response trong `Models/DTOs`.
2. Thêm interface trong `Repositories/Interfaces`, implementation trong
   `Repositories`; đăng ký scoped tại `Helpers/PersistenceServiceCollectionExtensions.cs`.
3. Thêm interface trong `Services/Interfaces`, implementation trong `Services`;
   đăng ký tại `Helpers/ApplicationServiceCollectionExtensions.cs`.
4. Tạo controller gọi interface service; đặt `[Authorize]` cho API cần đăng nhập.
5. Nếu có dữ liệu mới: thêm entity trong `Models/Entities`, `DbSet` trong context và
   `IEntityTypeConfiguration<T>` trong `Data/Configurations`. Đăng ký configuration
   bằng `builder.ApplyConfiguration(...)` trong `OnModelCreating`.
6. Chỉ tạo migration khi model/schema thực sự thay đổi; giữ nguyên migration
   đã áp dụng trước đó.

API lỗi dùng `ProblemDetails` / `ValidationProblemDetails`; API thành công trả
response DTO theo từng endpoint. Việc tổ chức lại thư mục không đổi route,
JSON contract, thời hạn token hoặc cấu trúc bảng hiện có.

Middleware lỗi sử dụng định dạng [ProblemDetails của ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-10.0).

## Local configuration

The PostgreSQL connection string and local JWT settings are read from
`appsettings.json`. This file is ignored by Git because it contains secrets.

```json
"Jwt": {
  "Issuer": "learn-english-backend",
  "Audience": "learn-english-client",
  "SigningKey": "your-secret-with-at-least-32-bytes",
  "AccessTokenLifetimeMinutes": 15,
  "RefreshTokenLifetimeDays": 30
}
```

Deployment environments can override these values with environment variables:

```text
Jwt__Issuer=learn-english-backend
Jwt__Audience=learn-english-client
Jwt__SigningKey=<secret>
Jwt__AccessTokenLifetimeMinutes=15
Jwt__RefreshTokenLifetimeDays=30
```

Never remove `appsettings.json` from `.gitignore` while it contains a connection
string or signing key. A local `dotnet publish` can still copy this ignored file
into the publish artifact, so protect the artifact and use a secret manager for
production deployments.

## Authentication endpoints

- `POST /api/auth/register`
- `POST /api/auth/login`
- `POST /api/auth/refresh`
- `POST /api/auth/revoke`
- `GET /api/auth/me` (requires `Authorization: Bearer <access-token>`)

## Learning endpoints

- `GET /api/learning` — danh sách bài học và 148 từ vựng, cho phép truy cập ẩn danh.
- `GET /api/learning/progress` — tiến độ của tài khoản hiện tại.
- `PUT /api/learning/progress/{wordId}` — đánh dấu/bỏ đánh dấu từ đã thuộc.
- `POST /api/learning/progress/import` — gộp tiến độ localStorage khi đăng nhập.

Migration `AddLearningContent` tạo các bảng `Lessons`, `VocabularyWords`,
`UserWordProgress` và seed toàn bộ dữ liệu cũ từ frontend vào PostgreSQL. Áp
dụng database và chạy API bằng:

```sh
dotnet ef database update
dotnet run --launch-profile http
```

Frontend local tại `http://localhost:5173` và `http://127.0.0.1:5173` đã được
cho phép qua CORS. Có thể cấu hình danh sách production qua
`Cors__AllowedOrigins__0`, `Cors__AllowedOrigins__1`, ...
