namespace EduCore.Domain.Enums;

public enum EnrollmentStatus
{
    Pending = 0,
    Active = 1,
    Suspended = 2,
    Withdrawn = 3,
    Graduated = 4
}

public enum PaymentStatus
{
    Pending = 0,
    Paid = 1,
    Overdue = 2,
    Cancelled = 3,
    Refunded = 4
}

public enum PaymentMethod
{
    Cash = 0,
    CreditCard = 1,
    DebitCard = 2,
    BankTransfer = 3,
    Check = 4,
    Online = 5
}

public enum GradeType
{
    Exam = 0,
    Quiz = 1,
    Homework = 2,
    Project = 3,
    Participation = 4,
    FinalExam = 5
}

public enum UserRole
{
    Admin = 0,
    Director = 1,
    Teacher = 2,
    Secretary = 3,
    Accountant = 4,
    Parent = 5,
    Student = 6
}

public enum GuardianType
{
    Mother = 0,
    Father = 1,
    Guardian = 2
}
