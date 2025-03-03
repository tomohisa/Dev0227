import { NextRequest, NextResponse } from 'next/server';
import serverAPI from '@/lib/server/api';
import { Student, RegisterStudentPayload } from '@/types/student';

export async function GET(request: NextRequest) {
  try {
    const searchParams = request.nextUrl.searchParams;
    const nameContains = searchParams.get('nameContains') || undefined;
    const studentIdContains = searchParams.get('studentIdContains') || undefined;
    
    const params: Record<string, string> = {};
    if (nameContains) params.nameContains = nameContains;
    if (studentIdContains) params.studentIdContains = studentIdContains;
    
    const students = await serverAPI.get<Student[]>('/api/students', params);
    return NextResponse.json(students);
  } catch (error: any) {
    console.error('Error fetching students:', error);
    return NextResponse.json(
      { error: error.response?.data?.detail || 'Failed to fetch students' },
      { status: error.response?.status || 500 }
    );
  }
}

export async function POST(request: NextRequest) {
  try {
    const body = await request.json();
    const student = await serverAPI.post<Student>('/api/students/register', body);
    return NextResponse.json(student);
  } catch (error: any) {
    console.error('Error creating student:', error);
    return NextResponse.json(
      { error: error.response?.data?.detail || 'Failed to create student' },
      { status: error.response?.status || 500 }
    );
  }
}
