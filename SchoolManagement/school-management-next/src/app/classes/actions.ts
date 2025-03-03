'use server'

import * as classService from '@/lib/server/services/classes';
import { CreateClassPayload, UpdateClassPayload } from '@/lib/server/services/classes';

export async function getClasses(nameContains?: string, classCodeContains?: string) {
    try {
        return {
            data: await classService.getClasses({ nameContains, classCodeContains }),
            error: null
        };
    } catch (error: any) {
        return {
            data: null,
            error: error.message
        };
    }
}

export async function createClass(data: CreateClassPayload) {
    try {
        await classService.createClass(data);
        return { error: null };
    } catch (error: any) {
        return { error: error.message };
    }
}

export async function updateClass(classId: string, data: Partial<UpdateClassPayload>) {
    try {
        await classService.updateClass(classId, data);
        return { error: null };
    } catch (error: any) {
        return { error: error.message };
    }
}

export async function deleteClass(classId: string) {
    try {
        await classService.deleteClass(classId);
        return { error: null };
    } catch (error: any) {
        return { error: error.message };
    }
}

export async function assignTeacherToClass(classId: string, teacherId: string) {
    try {
        await classService.assignTeacherToClass(classId, teacherId);
        return { error: null };
    } catch (error: any) {
        return { error: error.message };
    }
}

export async function removeTeacherFromClass(classId: string) {
    try {
        await classService.removeTeacherFromClass(classId);
        return { error: null };
    } catch (error: any) {
        return { error: error.message };
    }
}

export async function addStudentToClass(classId: string, studentId: string) {
    try {
        await classService.addStudentToClass(classId, studentId);
        return { error: null };
    } catch (error: any) {
        return { error: error.message };
    }
}

export async function removeStudentFromClass(classId: string, studentId: string) {
    try {
        await classService.removeStudentFromClass(classId, studentId);
        return { error: null };
    } catch (error: any) {
        return { error: error.message };
    }
}
