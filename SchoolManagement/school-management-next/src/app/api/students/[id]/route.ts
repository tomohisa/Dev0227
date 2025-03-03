import { NextRequest, NextResponse } from 'next/server';
import serverAPI from '@/lib/server/api';
import { Student, UpdateStudentPayload } from '@/types/student';

export async function GET(
  request: NextRequest,
  { params }: { params: { id: string } }
) {
  try {
    const student = await serverAPI.get<Student>(`/api/students/${params.id}`);
    return NextResponse.json(student);
  } catch (error: any) {
    console.error('Error fetching student:', error);
    return NextResponse.json(
      { error: error.response?.data?.detail || 'Failed to fetch student' },
      { status: error.response?.status || 500 }
    );
  }
}

export async function PUT(
  request: NextRequest,
  { params }: { params: { id: string } }
) {
  try {
    const body = await request.json();
    const updatePayload: UpdateStudentPayload = {
      studentId: params.id,
      ...body,
    };
    const result = await serverAPI.post<any>('/api/students/update', updatePayload);
    return NextResponse.json(result);
  } catch (error: any) {
    console.error('Error updating student:', error);
    return NextResponse.json(
      { error: error.response?.data?.detail || 'Failed to update student' },
      { status: error.response?.status || 500 }
    );
  }
}

export async function DELETE(
  request: NextRequest,
  { params }: { params: { id: string } }
) {
  try {
    const result = await serverAPI.post<any>('/api/students/delete', { studentId: params.id });
    return NextResponse.json(result);
  } catch (error: any) {
    console.error('Error deleting student:', error);
    return NextResponse.json(
      { error: error.response?.data?.detail || 'Failed to delete student' },
      { status: error.response?.status || 500 }
    );
  }
}
