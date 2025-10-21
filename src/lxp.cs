using System;
using System.Net;
using System.Collections.Generic;
using LxpAPI.DataClasses;

namespace LxpAPI {
    static class RequestBody {
        public static readonly string LogIn = "{{\"operationName\":\"SignIn\",\"variables\":{{\"input\":{{\"email\":\"{0}\",\"password\":\"{1}\"}}}},\"query\":\"query SignIn($input: SignInInput!) {{\\n  signIn(input: $input) {{\\n    user {{\\n      id\\n      isLead\\n      __typename\\n    }}\\n    accessToken\\n    __typename\\n  }}\\n}}\"}}";
        public static readonly string GetSchedule = "{{\"operationName\":\"ManyClassesForSchedule\",\"variables\":{{\"isAdministrationSchedule\":false,\"input\":{{\"page\":1,\"pageSize\":50,\"filters\":{{\"roles\":[\"STUDENT\",\"TEACHER\",\"STUDENT_PARENT\"],\"interval\":{{\"from\":\"{0}\",\"to\":\"{1}\"}}}}}}}},\"query\":\"query ManyClassesForSchedule($input: ManyClassesInput!, $isAdministrationSchedule: Boolean = false) {{\\n  manyClasses(input: $input) {{\\n    ...ClassInScheduleFragment\\n    __typename\\n  }}\\n}}\\n\\nfragment ClassInScheduleFragment on Class {{\\n  id\\n  from\\n  to\\n  name\\n  role\\n  isOnline\\n  isAutoMeetingLink\\n  meetingLink\\n  suborganizationId\\n  suborganization {{\\n    id\\n    name\\n    __typename\\n  }}\\n  retakingGroup {{\\n    id\\n    name\\n    disciplineId\\n    __typename\\n  }}\\n  discipline {{\\n    id\\n    name\\n    code\\n    archivedAt\\n    templateDiscipline {{\\n      id\\n      disciplinesGroup {{\\n        id\\n        name\\n        __typename\\n      }}\\n      __typename\\n    }}\\n    suborganization {{\\n      id\\n      organizationId\\n      __typename\\n    }}\\n    __typename\\n  }}\\n  learningGroup {{\\n    id\\n    name\\n    isArchived\\n    __typename\\n  }}\\n  classroom {{\\n    id\\n    name\\n    buildingArea {{\\n      id\\n      name\\n      __typename\\n    }}\\n    __typename\\n  }}\\n  teacher {{\\n    id\\n    user {{\\n      id\\n      firstName\\n      lastName\\n      middleName\\n      __typename\\n    }}\\n    __typename\\n  }}\\n  teachers {{\\n    id\\n    user {{\\n      id\\n      firstName\\n      lastName\\n      middleName\\n      __typename\\n    }}\\n    __typename\\n  }}\\n  flow {{\\n    id\\n    name\\n    learningGroups {{\\n      id\\n      name\\n      __typename\\n    }}\\n    __typename\\n  }}\\n  hasAttendance @include(if: $isAdministrationSchedule)\\n  errors {{\\n    ...ClassParamsErrorsFragment\\n    __typename\\n  }}\\n  ctpTopics {{\\n    id\\n    name\\n    __typename\\n  }}\\n  __typename\\n}}\\n\\nfragment ClassParamsErrorsFragment on ClassParamsErrors {{\\n  buildingArea\\n  classroom\\n  discipline\\n  teacher\\n  teachers\\n  classCount\\n  classTime\\n  learningGroup\\n  __typename\\n}}\"}}";
    }

    class LxpClient {
        string _email;
        string _password;
        string _token;

        public LxpClient(string email, string password){
            _email = email;
            _password = password;
        }

        public bool LogIn(){
            try{
                var response = RequestHelper.MakeAPIRequest<Response<SignInResponse>>("null", string.Format(RequestBody.LogIn, _email, _password));
                _token = response.data.signIn.accessToken;
                return true;
            }catch(LxpException){
                return false;
            }
        }

        string FormatDate(DateTime dateTime){
            return $"{dateTime.Year}-{dateTime.Month}-{dateTime.Day}";
        }

        public List<Class> GetSchedule(DateTime from, DateTime to){
            var response = RequestHelper.MakeAPIRequest<Response<ManyClassesResponse>>(_token, string.Format(RequestBody.GetSchedule, FormatDate(from), FormatDate(to)));
            return response.data.manyClasses;
        }
    }
}