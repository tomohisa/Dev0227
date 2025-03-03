import { NextRequest, NextResponse } from 'next/server';
import serverAPI from '@/lib/server/api';
import { AssignStudentToClassPayload, RemoveStudentFromClassPayload, Student } from '@/types/student';

export async function GET(request: NextRequest) {
  try {
    const classId = request.nextUrl.searchParams.get('classId');
    if (!classId) {
      return NextResponse.json(
        { error: 'Class ID is required' },
        { status: 400 }
      );
    }
    
    const students = await serverAPI.get<Student[]>(`/api/classes/${classId}/students`);
    return NextResponse.json(students);
  } catch (error: any) {
    console.error('Error fetching students by class:', error);
    return NextResponse.json(
      { error: error.response?.data?.detail || 'Failed to fetch students by class' },
      { status: error.response?.status || 500 }
    );
  }
}

export async function POST(request: NextRequest) {
  try {
    const body = await request.json();
    const result = await serverAPI.post<any>('/api/students/assigntoclass', body);
    return NextResponse.json(result);
  } catch (error: any) {
    console.error('Error assigning student to class:', error);
    return NextResponse.json(
      { error: error.response?.data?.detail || 'Failed to assign student to class' },
      { status: error.response?.status || 500 }
    );
  }
}

export async function DELETE(request: NextRequest) {
  try {
    const body = await request.json();
    const result = await serverAPI.post<any>('/api/students/removefromclass', body);
    return NextResponse.json(result);
  } catch (error: any) {
    console.error('Error removing student from class:', error);
    return NextResponse.json(
      { error: error.response?.data?.detail || 'Failed to remove student from class' },
      { status: error.response?.status || 500 }
    );
  }
}
