import serverAPI from '../api';
import { Teacher, RegisterTeacherPayload, UpdateTeacherPayload } from '@/types/teacher';
import { revalidatePath } from 'next/cache';

export async function getTeachers(params?: { nameContains?: string; subjectContains?: string }): Promise<Teacher[]> {
    try {
        const searchParams = new URLSearchParams();
        if (params?.nameContains) searchParams.append('nameContains', params.nameContains);
        if (params?.subjectContains) searchParams.append('subjectContains', params.subjectContains);

        const query = searchParams.toString() ? `?${searchParams.toString()}` : '';
        return await serverAPI.get<Teacher[]>(`/api/teachers${query}`);
    } catch (error: any) {
        console.error('Error fetching teachers:', error);
        throw new Error(error.response?.data?.detail || 'Failed to fetch teachers');
    }
}

export async function registerTeacher(data: RegisterTeacherPayload) {
    try {
        const result = await serverAPI.post<Teacher>('/api/teachers/register', data);
        revalidatePath('/teachers');
        return result;
    } catch (error: any) {
        console.error('Error registering teacher:', error);
        throw new Error(error.response?.data?.detail || 'Failed to register teacher');
    }
}

export async function updateTeacher(teacherId: string, data: Partial<UpdateTeacherPayload>) {
    try {
        const result = await serverAPI.post('/api/teachers/update', {
            teacherId,
            ...data,
        });
        revalidatePath('/teachers');
        return result;
    } catch (error: any) {
        console.error('Error updating teacher:', error);
        throw new Error(error.response?.data?.detail || 'Failed to update teacher');
    }
}

export async function deleteTeacher(teacherId: string) {
    try {
        const result = await serverAPI.post('/api/teachers/delete', { teacherId });
        revalidatePath('/teachers');
        return result;
    } catch (error: any) {
        console.error('Error deleting teacher:', error);
        throw new Error(error.response?.data?.detail || 'Failed to delete teacher');
    }
}

export async function assignTeacherToClass(teacherId: string, classId: string) {
    try {
        const result = await serverAPI.post('/api/teachers/assigntoclass', { teacherId, classId });
        revalidatePath('/teachers');
        return result;
    } catch (error: any) {
        console.error('Error assigning teacher to class:', error);
        throw new Error(error.response?.data?.detail || 'Failed to assign teacher to class');
    }
}

export async function removeTeacherFromClass(teacherId: string, classId: string) {
    try {
        const result = await serverAPI.post('/api/teachers/removefromclass', { teacherId, classId });
        revalidatePath('/teachers');
        return result;
    } catch (error: any) {
        console.error('Error removing teacher from class:', error);
        throw new Error(error.response?.data?.detail || 'Failed to remove teacher from class');
    }
}
