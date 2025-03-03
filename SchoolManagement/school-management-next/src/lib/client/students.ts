import { Student, RegisterStudentPayload, UpdateStudentPayload } from '@/types/student';

export async function getStudents(params?: { nameContains?: string; studentIdContains?: string }) {
  const searchParams = new URLSearchParams();
  if (params?.nameContains) searchParams.append('nameContains', params.nameContains);
  if (params?.studentIdContains) searchParams.append('studentIdContains', params.studentIdContains);

  const query = searchParams.toString() ? `?${searchParams.toString()}` : '';
  const response = await fetch(`/api/students${query}`);
  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.error || 'Failed to fetch students');
  }
  return response.json() as Promise<Student[]>;
}

export async function registerStudent(data: RegisterStudentPayload) {
  const response = await fetch('/api/students', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  
  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.error || 'Failed to register student');
  }
  return response.json();
}

export async function updateStudent(studentId: string, data: Partial<UpdateStudentPayload>) {
  const response = await fetch(`/api/students/${studentId}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  
  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.error || 'Failed to update student');
  }
  return response.json();
}

export async function deleteStudent(studentId: string) {
  const response = await fetch(`/api/students/${studentId}`, {
    method: 'DELETE',
  });
  
  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.error || 'Failed to delete student');
  }
  return response.json();
}

export async function assignStudentToClass(studentId: string, classId: string) {
  const response = await fetch('/api/students/class', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ studentId, classId }),
  });
  
  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.error || 'Failed to assign student to class');
  }
  return response.json();
}

export async function removeStudentFromClass(studentId: string) {
  const response = await fetch('/api/students/class', {
    method: 'DELETE',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ studentId }),
  });
  
  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.error || 'Failed to remove student from class');
  }
  return response.json();
}
