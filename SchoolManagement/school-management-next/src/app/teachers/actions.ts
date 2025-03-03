'use server'

import * as teacherService from '@/lib/server/services/teachers';
import { RegisterTeacherPayload, UpdateTeacherPayload } from '@/types/teacher';

export async function getTeachers(nameContains?: string, subjectContains?: string) {
    try {
        return {
            data: await teacherService.getTeachers({ nameContains, subjectContains }),
            error: null
        };
    } catch (error: any) {
        return {
            data: null,
            error: error.message
        };
    }
}

export async function registerTeacher(data: RegisterTeacherPayload) {
    try {
        await teacherService.registerTeacher(data);
        return { error: null };
    } catch (error: any) {
        return { error: error.message };
    }
}

export async function updateTeacher(teacherId: string, data: Partial<UpdateTeacherPayload>) {
    try {
        await teacherService.updateTeacher(teacherId, data);
        return { error: null };
    } catch (error: any) {
        return { error: error.message };
    }
}

export async function deleteTeacher(teacherId: string) {
    try {
        await teacherService.deleteTeacher(teacherId);
        return { error: null };
    } catch (error: any) {
        return { error: error.message };
    }
}

export async function assignTeacherToClass(teacherId: string, classId: string) {
    try {
        await teacherService.assignTeacherToClass(teacherId, classId);
        return { error: null };
    } catch (error: any) {
        return { error: error.message };
    }
}

export async function removeTeacherFromClass(teacherId: string, classId: string) {
    try {
        await teacherService.removeTeacherFromClass(teacherId, classId);
        return { error: null };
    } catch (error: any) {
        return { error: error.message };
    }
}
