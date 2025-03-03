import { Teacher, RegisterTeacherPayload, UpdateTeacherPayload } from '@/types/teacher';

export async function getTeachers(params?: { nameContains?: string; subjectContains?: string }) {
  const searchParams = new URLSearchParams();
  if (params?.nameContains) searchParams.append('nameContains', params.nameContains);
  if (params?.subjectContains) searchParams.append('subjectContains', params.subjectContains);

  const query = searchParams.toString() ? `?${searchParams.toString()}` : '';
  const response = await fetch(`/api/teachers${query}`);
  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.error || 'Failed to fetch teachers');
  }
  return response.json() as Promise<Teacher[]>;
}

export async function registerTeacher(data: RegisterTeacherPayload) {
  const response = await fetch('/api/teachers', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  
  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.error || 'Failed to register teacher');
  }
  return response.json();
}

export async function updateTeacher(teacherId: string, data: Partial<UpdateTeacherPayload>) {
  const response = await fetch(`/api/teachers/${teacherId}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  
  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.error || 'Failed to update teacher');
  }
  return response.json();
}

export async function deleteTeacher(teacherId: string) {
  const response = await fetch(`/api/teachers/${teacherId}`, {
    method: 'DELETE',
  });
  
  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.error || 'Failed to delete teacher');
  }
  return response.json();
}

export async function assignTeacherToClass(teacherId: string, classId: string) {
  const response = await fetch('/api/teachers/class', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ teacherId, classId }),
  });
  
  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.error || 'Failed to assign teacher to class');
  }
  return response.json();
}

export async function removeTeacherFromClass(teacherId: string, classId: string) {
  const response = await fetch('/api/teachers/class', {
    method: 'DELETE',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ teacherId, classId }),
  });
  
  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.error || 'Failed to remove teacher from class');
  }
  return response.json();
}
