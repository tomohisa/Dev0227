import { NextRequest, NextResponse } from 'next/server';
import serverAPI from '@/lib/server/api';
import { Teacher } from '@/types/teacher';

export async function GET(request: NextRequest) {
  try {
    const classId = request.nextUrl.searchParams.get('classId');
    if (!classId) {
      return NextResponse.json(
        { error: 'Class ID is required' },
        { status: 400 }
      );
    }
    
    const teachers = await serverAPI.get<Teacher[]>(`/api/classes/${classId}/teachers`);
    return NextResponse.json(teachers);
  } catch (error: any) {
    console.error('Error fetching teachers by class:', error);
    return NextResponse.json(
      { error: error.response?.data?.detail || 'Failed to fetch teachers by class' },
      { status: error.response?.status || 500 }
    );
  }
}

export async function POST(request: NextRequest) {
  try {
    const body = await request.json();
    const result = await serverAPI.post<any>('/api/teachers/assigntoclass', body);
    return NextResponse.json(result);
  } catch (error: any) {
    console.error('Error assigning teacher to class:', error);
    return NextResponse.json(
      { error: error.response?.data?.detail || 'Failed to assign teacher to class' },
      { status: error.response?.status || 500 }
    );
  }
}

export async function DELETE(request: NextRequest) {
  try {
    const body = await request.json();
    const result = await serverAPI.post<any>('/api/teachers/removefromclass', body);
    return NextResponse.json(result);
  } catch (error: any) {
    console.error('Error removing teacher from class:', error);
    return NextResponse.json(
      { error: error.response?.data?.detail || 'Failed to remove teacher from class' },
      { status: error.response?.status || 500 }
    );
  }
}
