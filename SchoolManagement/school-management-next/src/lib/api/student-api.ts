import apiClient from './api-client';

export interface Student {
  studentId: string;
  name: string;
  studentIdNumber: string;
  dateOfBirth: string;
  email: string;
  phoneNumber: string;
  address: string;
  classId: string | null;
  age: number;
}

export interface RegisterStudentRequest {
  name: string;
  studentId: string;
  dateOfBirth: string;
  email: string;
  phoneNumber: string;
  address: string;
}

export interface UpdateStudentRequest {
  studentId: string;
  name?: string;
  email?: string;
  phoneNumber?: string;
  address?: string;
}

export interface AssignStudentToClassRequest {
  studentId: string;
  classId: string;
}

export interface RemoveStudentFromClassRequest {
  studentId: string;
}

export interface DeleteStudentRequest {
  studentId: string;
}

export const StudentApi = {
  getStudents: async (nameContains?: string, studentIdContains?: string): Promise<Student[]> => {
    let url = '/api/students';
    const params = new URLSearchParams();
    
    if (nameContains) {
      params.append('nameContains', nameContains);
    }
    
    if (studentIdContains) {
      params.append('studentIdContains', studentIdContains);
    }
    
    if (params.toString()) {
      url += `?${params.toString()}`;
    }
    
    const response = await apiClient.get<Student[]>(url);
    return response.data;
  },
  
  getStudentById: async (studentId: string): Promise<Student> => {
    const response = await apiClient.get<Student>(`/api/students/${studentId}`);
    return response.data;
  },
  
  getStudentsByClassId: async (classId: string): Promise<Student[]> => {
    const response = await apiClient.get<Student[]>(`/api/classes/${classId}/students`);
    return response.data;
  },
  
  registerStudent: async (request: RegisterStudentRequest): Promise<any> => {
    const response = await apiClient.post('/api/students/register', request);
    return response.data;
  },
  
  updateStudent: async (request: UpdateStudentRequest): Promise<any> => {
    const response = await apiClient.post('/api/students/update', request);
    return response.data;
  },
  
  deleteStudent: async (studentId: string): Promise<any> => {
    const request: DeleteStudentRequest = { studentId };
    const response = await apiClient.post('/api/students/delete', request);
    return response.data;
  },
  
  assignStudentToClass: async (studentId: string, classId: string): Promise<any> => {
    const request: AssignStudentToClassRequest = { studentId, classId };
    const response = await apiClient.post('/api/students/assigntoclass', request);
    return response.data;
  },
  
  removeStudentFromClass: async (studentId: string): Promise<any> => {
    const request: RemoveStudentFromClassRequest = { studentId };
    const response = await apiClient.post('/api/students/removefromclass', request);
    return response.data;
  }
};
