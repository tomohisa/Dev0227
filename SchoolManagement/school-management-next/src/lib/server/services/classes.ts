import serverAPI from '../api';
import { revalidatePath } from 'next/cache';

export interface Class {
  classId: string;
  name: string;
  classCode: string;
  description: string;
  teacherId?: string;
  studentIds: string[];
}

export interface CreateClassPayload {
  name: string;
  classCode: string;
  description: string;
}

export interface UpdateClassPayload {
  name?: string;
  classCode?: string;
  description?: string;
}

export async function getClasses(params?: { nameContains?: string; classCodeContains?: string }): Promise<Class[]> {
  try {
    const searchParams = new URLSearchParams();
    if (params?.nameContains) searchParams.append('nameContains', params.nameContains);
    if (params?.classCodeContains) searchParams.append('classCodeContains', params.classCodeContains);

    const query = searchParams.toString() ? `?${searchParams.toString()}` : '';
    return await serverAPI.get<Class[]>(`/api/classes${query}`);
  } catch (error: any) {
    console.error('Error fetching classes:', error);
    throw new Error(error.response?.data?.detail || 'Failed to fetch classes');
  }
}

export async function createClass(data: CreateClassPayload) {
  try {
    const result = await serverAPI.post<Class>('/api/classes/create', data);
    revalidatePath('/classes');
    return result;
  } catch (error: any) {
    console.error('Error creating class:', error);
    throw new Error(error.response?.data?.detail || 'Failed to create class');
  }
}

export async function updateClass(classId: string, data: Partial<UpdateClassPayload>) {
  try {
    const result = await serverAPI.post('/api/classes/update', {
      classId,
      ...data,
    });
    revalidatePath('/classes');
    return result;
  } catch (error: any) {
    console.error('Error updating class:', error);
    throw new Error(error.response?.data?.detail || 'Failed to update class');
  }
}

export async function deleteClass(classId: string) {
  try {
    const result = await serverAPI.post('/api/classes/delete', { classId });
    revalidatePath('/classes');
    return result;
  } catch (error: any) {
    console.error('Error deleting class:', error);
    throw new Error(error.response?.data?.detail || 'Failed to delete class');
  }
}

export async function assignTeacherToClass(classId: string, teacherId: string) {
  try {
    const result = await serverAPI.post('/api/classes/assignteacher', { classId, teacherId });
    revalidatePath('/classes');
    return result;
  } catch (error: any) {
    console.error('Error assigning teacher to class:', error);
    throw new Error(error.response?.data?.detail || 'Failed to assign teacher to class');
  }
}

export async function removeTeacherFromClass(classId: string) {
  try {
    const result = await serverAPI.post('/api/classes/removeteacher', { classId });
    revalidatePath('/classes');
    return result;
  } catch (error: any) {
    console.error('Error removing teacher from class:', error);
    throw new Error(error.response?.data?.detail || 'Failed to remove teacher from class');
  }
}

export async function addStudentToClass(classId: string, studentId: string) {
  try {
    const result = await serverAPI.post('/api/classes/addstudent', { classId, studentId });
    revalidatePath('/classes');
    return result;
  } catch (error: any) {
    console.error('Error adding student to class:', error);
    throw new Error(error.response?.data?.detail || 'Failed to add student to class');
  }
}

export async function removeStudentFromClass(classId: string, studentId: string) {
  try {
    const result = await serverAPI.post('/api/classes/removestudent', { classId, studentId });
    revalidatePath('/classes');
    return result;
  } catch (error: any) {
    console.error('Error removing student from class:', error);
    throw new Error(error.response?.data?.detail || 'Failed to remove student from class');
  }
}
