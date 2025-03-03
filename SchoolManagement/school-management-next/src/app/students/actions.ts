'use server'

import * as studentService from '@/lib/server/services/students';
import { RegisterStudentPayload, UpdateStudentPayload } from '@/types/student';

export async function getStudents(nameContains?: string, studentIdContains?: string) {
    try {
        return {
            data: await studentService.getStudents({ nameContains, studentIdContains }),
            error: null
        };
    } catch (error: any) {
        return {
            data: null,
            error: error.message
        };
    }
}

export async function registerStudent(data: RegisterStudentPayload) {
    try {
        await studentService.registerStudent(data);
        return { error: null };
    } catch (error: any) {
        return { error: error.message };
    }
}

export async function updateStudent(studentId: string, data: Partial<UpdateStudentPayload>) {
    try {
        await studentService.updateStudent(studentId, data);
        return { error: null };
    } catch (error: any) {
        return { error: error.message };
    }
}

export async function deleteStudent(studentId: string) {
    try {
        await studentService.deleteStudent(studentId);
        return { error: null };
    } catch (error: any) {
        return { error: error.message };
    }
}

export async function assignStudentToClass(studentId: string, classId: string) {
    try {
        await studentService.assignStudentToClass(studentId, classId);
        return { error: null };
    } catch (error: any) {
        return { error: error.message };
    }
}

export async function removeStudentFromClass(studentId: string) {
    try {
        await studentService.removeStudentFromClass(studentId);
        return { error: null };
    } catch (error: any) {
        return { error: error.message };
    }
}
