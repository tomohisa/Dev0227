import apiClient from './api-client';

export interface Class {
  classId: string;
  name: string;
  classCode: string;
  description: string;
  teacherId: string | null;
  studentIds: string[];
}

export interface CreateClassRequest {
  name: string;
  classCode: string;
  description: string;
}

export interface UpdateClassRequest {
  classId: string;
  name?: string;
  classCode?: string;
  description?: string;
}

export interface AssignTeacherToClassRequest {
  classId: string;
  teacherId: string;
}

export interface RemoveTeacherFromClassRequest {
  classId: string;
}

export interface AddStudentToClassRequest {
  classId: string;
  studentId: string;
}

export interface RemoveStudentFromClassRequest {
  classId: string;
  studentId: string;
}

export interface DeleteClassRequest {
  classId: string;
}

export const ClassApi = {
  getClasses: async (nameContains?: string, classCodeContains?: string): Promise<Class[]> => {
    let url = '/api/classes';
    const params = new URLSearchParams();
    
    if (nameContains) {
      params.append('nameContains', nameContains);
    }
    
    if (classCodeContains) {
      params.append('classCodeContains', classCodeContains);
    }
    
    if (params.toString()) {
      url += `?${params.toString()}`;
    }
    
    const response = await apiClient.get<Class[]>(url);
    return response.data;
  },
  
  getClassById: async (classId: string): Promise<Class> => {
    const response = await apiClient.get<Class>(`/api/classes/${classId}`);
    return response.data;
  },
  
  getClassesByTeacherId: async (teacherId: string): Promise<Class[]> => {
    const response = await apiClient.get<Class[]>(`/api/teachers/${teacherId}/classes`);
    return response.data;
  },
  
  getClassesByStudentId: async (studentId: string): Promise<Class[]> => {
    const response = await apiClient.get<Class[]>(`/api/students/${studentId}/classes`);
    return response.data;
  },
  
  createClass: async (request: CreateClassRequest): Promise<any> => {
    const response = await apiClient.post('/api/classes/create', request);
    return response.data;
  },
  
  updateClass: async (request: UpdateClassRequest): Promise<any> => {
    const response = await apiClient.post('/api/classes/update', request);
    return response.data;
  },
  
  deleteClass: async (classId: string): Promise<any> => {
    const request: DeleteClassRequest = { classId };
    const response = await apiClient.post('/api/classes/delete', request);
    return response.data;
  },
  
  assignTeacherToClass: async (classId: string, teacherId: string): Promise<any> => {
    const request: AssignTeacherToClassRequest = { classId, teacherId };
    const response = await apiClient.post('/api/classes/assignteacher', request);
    return response.data;
  },
  
  removeTeacherFromClass: async (classId: string): Promise<any> => {
    const request: RemoveTeacherFromClassRequest = { classId };
    const response = await apiClient.post('/api/classes/removeteacher', request);
    return response.data;
  },
  
  addStudentToClass: async (classId: string, studentId: string): Promise<any> => {
    const request: AddStudentToClassRequest = { classId, studentId };
    const response = await apiClient.post('/api/classes/addstudent', request);
    return response.data;
  },
  
  removeStudentFromClass: async (classId: string, studentId: string): Promise<any> => {
    const request: RemoveStudentFromClassRequest = { classId, studentId };
    const response = await apiClient.post('/api/classes/removestudent', request);
    return response.data;
  }
};
