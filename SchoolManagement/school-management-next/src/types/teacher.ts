export interface Teacher {
  teacherId: string;
  name: string;
  subject: string;
  email: string;
  phoneNumber: string;
  address: string;
  classIds: string[] | null;
}

export interface RegisterTeacherPayload {
  name: string;
  teacherId: string;
  subject: string;
  email: string;
  phoneNumber: string;
  address: string;
}

export interface UpdateTeacherPayload {
  teacherId: string;
  name?: string;
  subject?: string;
  email?: string;
  phoneNumber?: string;
  address?: string;
}

export interface AssignTeacherToClassPayload {
  teacherId: string;
  classId: string;
}

export interface RemoveTeacherFromClassPayload {
  teacherId: string;
  classId: string;
}
