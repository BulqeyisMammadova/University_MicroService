using Auth.Service.Core.Enums;

namespace Auth.Service.Business.Extensions;

public static class RoleExtensions
{
    public static string[] GetPermissions(this Role role)
    {
        return role switch
        {
            Role.Admin => new[]
            {
                "universities.view", "universities.manage",
                "teachers.view", "teachers.manage",
                "subjects.view", "subjects.manage",
                "students.view", "students.manage",
                "users.view", "users.manage"
            },

            Role.Teacher => new[]
            {
                "universities.view",
                "teachers.view",
                "teacher-phones.view", "teacher-phones.manage",
                "subjects.view", "subjects.manage",
                "students.view"
            },

            Role.Student => new[]
            {
                "universities.view",
                "teachers.view",
                "subjects.view",
                "students.view"
            },

            _ => Array.Empty<string>()
        };
    }
}