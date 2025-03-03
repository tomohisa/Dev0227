import { NextRequest, NextResponse } from 'next/server';
import serverAPI from '@/lib/server/api';
import { Teacher } from '@/types/teacher';

export async function GET(
  request: NextRequest,
  { params }: { params: { id: string } }
) {
  try {
    const teacher = await serverAPI.get<Teacher>(`/api/teachers/${params.id}`);
    return NextResponse.json(teacher);
  } catch (error: any) {
    console.error('Error fetching teacher:', error);
    return NextResponse.json(
      { error: error.response?.data?.detail || 'Failed to fetch teacher' },
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
    const updatePayload = {
      teacherId: params.id,
      ...body,
    };
    const result = await serverAPI.post<any>('/api/teachers/update', updatePayload);
    return NextResponse.json(result);
  } catch (error: any) {
    console.error('Error updating teacher:', error);
    return NextResponse.json(
      { error: error.response?.data?.detail || 'Failed to update teacher' },
      { status: error.response?.status || 500 }
    );
  }
}

export async function DELETE(
  request: NextRequest,
  { params }: { params: { id: string } }
) {
  try {
    const result = await serverAPI.post<any>('/api/teachers/delete', { teacherId: params.id });
    return NextResponse.json(result);
  } catch (error: any) {
    console.error('Error deleting teacher:', error);
    return NextResponse.json(
      { error: error.response?.data?.detail || 'Failed to delete teacher' },
      { status: error.response?.status || 500 }
    );
  }
}
