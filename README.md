🚀 JoDentify - Dental Clinic SaaS API
أهلاً بك في (JoDentify)! ده الـ Backend API (العقل) لنظام إدارة عيادات أسنان (SaaS) متكامل، مبني باستخدام .NET 8 و Clean Architecture.

الـ API ده جاهز 100% للعمل، وفيه كل الفيتشرز (الميزات) من اليوم الأول لليوم السابع اللي إحنا بنيناها مع بعض.

✨ الميزات الرئيسية (Key Features)
الـ API ده بيقدم 7 ميزات أساسية (Modules)، وكلها متأمنة بنظام الـ Multi-Tenancy (عشان كل عيادة تشوف الداتا بتاعتها بس):

1. 🔐 نظام الدخول والتسجيل (Authentication)
تسجيل حساب جديد (Register) كـ "صاحب عيادة" (Owner).

الـ API بينشئ "عيادة" (Clinic) جديدة أوتوماتيك مع التسجيل.

تسجيل الدخول (Login) والحصول على (JWT Token) آمن.

الـ Token بيحتوي على الـ ClinicId والـ UserId (أساس نظام الأمان).

2. 🏢 نظام تعدد العيادات (Multi-Tenancy)
النظام بالكامل مبني على ClinicId.

مستحيل أي يوزر (دكتور أو ريسيبشن) يشوف داتا (مرضى، مواعيد، فواتير) تابعة لعيادة تانية.

كل الـ Services (الـ "عقل") بتستخدم الـ clinicId الموجود في الـ Token عشان تفلتر الداتا أوتوماتيك.

3. 🧑‍🤝‍🧑 إدارة المرضى (Patient Management)
إضافة، تعديل، جلب، ومسح (Soft Delete) للمرضى.

كل مريض مربوط إجبارياً بـ ClinicId.

Validation احترافي (زي Gender لازم يكون Male, Female, Kids).

4. 📅 إدارة المواعيد (Appointment Management)
إنشاء، تعديل، جلب، ومسح (Soft Delete) للمواعيد.

الميعاد بيربط 3 جداول ببعض: Patient (المريض)، ApplicationUser (الدكتور)، و Clinic (العيادة).

نظام AppointmentStatus (محجوز، مؤكد، ملغي... إلخ).

5. 🧾 إدارة الخدمات (Clinic Services)
إضافة، تعديل، جلب، ومسح (Soft Delete) لـ "قايمة أسعار" العيادة.

كل خدمة (زي "تنضيف جير") ليها سعرها الخاص بالعيادة دي.

6. 💸 نظام الفواتير والدفع (Billing & Payments)
دعم الدفع الآجل (Partially Paid): أهم فيتشر طلبتها.

إنشاء "فاتورة" (Invoice) مربوطة بخدمات ومريض.

تسجيل "دفعات" (Payments) على الفاتورة (زي "دفع 200 جنيه من أصل 500").

السيستم بيحسب الـ AmountDue (المتبقي) وبيغير حالة الفاتورة لـ PartiallyPaid أوتوماتيك.

دعم طرق دفع مختلفة (Cash, CreditCard, VodafoneCash).

7. 📊 لوحة التحكم (Dashboard)
Endpoints جاهزة عشان تحسب "الإحصائيات" للعيادة دي بس.

إجمالي الدخل، إجمالي المتبقي (الآجل)، عدد المرضى الجدد، عدد المواعيد اليوم.

Endpoint عشان يجيب "آخر 5 مرضى" اتسجلوا.

🛠️ التقنيات المستخدمة (Tech Stack)
Backend: .NET 8 Web API

Architecture: Clean Architecture (Core, Application, Infrastructure, API)

Database: MS SQL Server (Code-First)

ORM: Entity Framework Core 8 (EF Core)

Authentication: ASP.NET Core Identity (Token-Based - JWT)

Design Patterns: Repository Pattern, Service Layer, DTOs

Tools: AutoMapper (للتحويل بين DTOs و Entities)

🚀 كيفية تشغيل المشروع (How to Run)
لأن المشروع ده متقسم لـ 4 مشاريع، إنت محتاج تشغله من الـ Solution Directory (الفولدر الأساسي).

اتأكد من الإعدادات:

اتأكد إن الـ ConnectionStrings في ملف JoDentify.API/appsettings.Development.json مظبوطة على الـ SQL Server بتاعك.

اتأكد إن الـ JWT:Key موجودة في نفس الملف.

شغل الـ API:

افتح Terminal (زي PM أو cmd) واقف في الفولدر الأساسي (E:\JoDentifySaaS).

نفذ الأمر ده:

Bash

dotnet run --project JoDentify.API --launch-profile http
افتح الـ Tester:

الـ API هيشتغل على http://localhost:5000.

افتح المتصفح وروح على: http://localhost:5000/swagger

🔐 إعدادات البيئة (Environment Setup)
الـ launchSettings.json مظبوط إنه يشغل الـ http profile.

الـ http profile مظبوط إنه يشغل ASPNETCORE_ENVIRONMENT كـ Development.

الأهم: كل الإعدادات السرية (الداتابيز والـ JWT Key) موجودة في appsettings.Development.json عشان الـ appsettings.json الأساسي يفضل نضيف.