import apiClient from './api-client';

export interface Teacher {
  teacherId: string;
  name: string;
  teacherIdNumber: string;
  email: string;
  phoneNumber: string;
  address: string;
  subject: string;
  classIds: string[];
}

export interface RegisterTeacherRequest {
  name: string;
  teacherId: string;
  email: string;
  phoneNumber: string;
  address: string;
  subject: string;
}

export interface UpdateTeacherRequest {
  teacherId: string;
  name?: string;
  email?: string;
  phoneNumber?: string;
  address?: string;
  subject?: string;
}

export interface AssignTeacherToClassRequest {
  teacherId: string;
  classId: string;
}

export interface RemoveTeacherFromClassRequest {
  teacherId: string;
  classId: string;
}

export interface DeleteTeacherRequest {
  teacherId: string;
}

export const TeacherApi = {
  getTeachers: async (nameContains?: string, subjectContains?: string): Promise<Teacher[]> => {
    let url = '/api/teachers';
    const params = new URLSearchParams();
    
    if (nameContains) {
      params.append('nameContains', nameContains);
    }
    
    if (subjectContains) {
      params.append('subjectContains', subjectContains);
    }
    
    if (params.toString()) {
      url += `?${params.toString()}`;
    }
    
    const response = await apiClient.get<Teacher[]>(url);
    return response.data;
  },
  
  getTeacherById: async (teacherId: string): Promise<Teacher> => {
    const response = await apiClient.get<Teacher>(`/api/teachers/${teacherId}`);
    return response.data;
  },
  
  getTeachersByClassId: async (classId: string): Promise<Teacher[]> => {
    const response = await apiClient.get<Teacher[]>(`/api/classes/${classId}/teachers`);
    return response.data;
  },
  
  registerTeacher: async (request: RegisterTeacherRequest): Promise<any> => {
    const response = await apiClient.post('/api/teachers/register', request);
    return response.data;
  },
  
  updateTeacher: async (request: UpdateTeacherRequest): Promise<any> => {
    const response = await apiClient.post('/api/teachers/update', request);
    return response.data;
  },
  
  deleteTeacher: async (teacherId: string): Promise<any> => {
    const request: DeleteTeacherRequest = { teacherId };
    const response = await apiClient.post('/api/teachers/delete', request);
    return response.data;
  },
  
  assignTeacherToClass: async (teacherId: string, classId: string): Promise<any> => {
    const request: AssignTeacherToClassRequest = { teacherId, classId };
    const response = await apiClient.post('/api/teachers/assigntoclass', request);
    return response.data;
  },
  
  removeTeacherFromClass: async (teacherId: string, classId: string): Promise<any> => {
    const request: RemoveTeacherFromClassRequest = { teacherId, classId };
    const response = await apiClient.post('/api/teachers/removefromclass', request);
    return response.data;
  }
};
