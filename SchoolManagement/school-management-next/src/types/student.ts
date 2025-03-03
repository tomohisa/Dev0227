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

export interface RegisterStudentPayload {
  name: string;
  studentId: string;
  dateOfBirth: string;
  email: string;
  phoneNumber: string;
  address: string;
}

export interface UpdateStudentPayload {
  studentId: string;
  name?: string;
  email?: string;
  phoneNumber?: string;
  address?: string;
}

export interface AssignStudentToClassPayload {
  studentId: string;
  classId: string;
}

export interface RemoveStudentFromClassPayload {
  studentId: string;
}
