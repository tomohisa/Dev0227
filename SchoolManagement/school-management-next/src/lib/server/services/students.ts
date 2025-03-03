import serverAPI from '../api';
import { Student, RegisterStudentPayload, UpdateStudentPayload } from '@/types/student';
import { revalidatePath } from 'next/cache';

export async function getStudents(params?: { nameContains?: string; studentIdContains?: string }): Promise<Student[]> {
    try {
        const searchParams = new URLSearchParams();
        if (params?.nameContains) searchParams.append('nameContains', params.nameContains);
        if (params?.studentIdContains) searchParams.append('studentIdContains', params.studentIdContains);

        const query = searchParams.toString() ? `?${searchParams.toString()}` : '';
        return await serverAPI.get<Student[]>(`/api/students${query}`);
    } catch (error: any) {
        console.error('Error fetching students:', error);
        throw new Error(error.response?.data?.detail || 'Failed to fetch students');
    }
}

export async function registerStudent(data: RegisterStudentPayload) {
    try {
        const result = await serverAPI.post<Student>('/api/students/register', data);
        revalidatePath('/students');
        return result;
    } catch (error: any) {
        console.error('Error registering student:', error);
        throw new Error(error.response?.data?.detail || 'Failed to register student');
    }
}

export async function updateStudent(studentId: string, data: Partial<UpdateStudentPayload>) {
    try {
        const result = await serverAPI.post('/api/students/update', {
            studentId,
            ...data,
        });
        revalidatePath('/students');
        return result;
    } catch (error: any) {
        console.error('Error updating student:', error);
        throw new Error(error.response?.data?.detail || 'Failed to update student');
    }
}

export async function deleteStudent(studentId: string) {
    try {
        const result = await serverAPI.post('/api/students/delete', { studentId });
        revalidatePath('/students');
        return result;
    } catch (error: any) {
        console.error('Error deleting student:', error);
        throw new Error(error.response?.data?.detail || 'Failed to delete student');
    }
}

export async function assignStudentToClass(studentId: string, classId: string) {
    try {
        const result = await serverAPI.post('/api/students/assigntoclass', { studentId, classId });
        revalidatePath('/students');
        return result;
    } catch (error: any) {
        console.error('Error assigning student to class:', error);
        throw new Error(error.response?.data?.detail || 'Failed to assign student to class');
    }
}

export async function removeStudentFromClass(studentId: string) {
    try {
        const result = await serverAPI.post('/api/students/removefromclass', { studentId });
        revalidatePath('/students');
        return result;
    } catch (error: any) {
        console.error('Error removing student from class:', error);
        throw new Error(error.response?.data?.detail || 'Failed to remove student from class');
    }
}
