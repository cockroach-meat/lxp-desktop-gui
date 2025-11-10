using System;
using System.Collections.Generic;

namespace LxpAPI.DataClasses {
    public class Response<T> {
        public T data;
    }

    public class User {
        public string id;
        public string firstName;
        public string lastName;
        public string middleName;
    }

    public class SignInResponse {
        public SignIn signIn;
    }

    public class SignIn {
        public User user;
        public string accessToken;
    }

    public class ManyClassesResponse {
        public List<Class> manyClasses;
    }

    public class GetNotificationsResponse {
        public Notifications notifications;
    }

    public class Notifications {
        public List<Notification> items;
    }

    public class Notification {
        public string id;
        public string title;
        public string body;
        public string createdAt;
        public bool isRead;
    }

    public class Class {
        public string id;
        public string from;
        public string to;
        public bool isOnline;
        public string suborganizationId;
        public Discipline discipline;
        public NamedObject learningGroup;
        public Classroom classroom;
        public List<Teacher> teachers;
    }

    public class Discipline {
        public string id;
        public string name;
        public string code;
    }
    
    public class NamedObject {
        public string id;
        public string name;
    }

    public class Teacher {
        public string id;
        public User user;
    }

    public class Classroom {
        public string id;
        public string name;
        public NamedObject buildingArea;
    }
}