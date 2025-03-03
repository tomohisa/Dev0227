import { NextRequest, NextResponse } from 'next/server';
import serverAPI from '@/lib/server/api';
import { Teacher } from '@/types/teacher';

export async function GET(request: NextRequest) {
  try {
    const searchParams = request.nextUrl.searchParams;
    const nameContains = searchParams.get('nameContains') || undefined;
    const subjectContains = searchParams.get('subjectContains') || undefined;
    
    const params: Record<string, string> = {};
    if (nameContains) params.nameContains = nameContains;
    if (subjectContains) params.subjectContains = subjectContains;
    
    const teachers = await serverAPI.get<Teacher[]>('/api/teachers', params);
    return NextResponse.json(teachers);
  } catch (error: any) {
    console.error('Error fetching teachers:', error);
    return NextResponse.json(
      { error: error.response?.data?.detail || 'Failed to fetch teachers' },
      { status: error.response?.status || 500 }
    );
  }
}

export async function POST(request: NextRequest) {
  try {
    const body = await request.json();
    const teacher = await serverAPI.post<Teacher>('/api/teachers/register', body);
    return NextResponse.json(teacher);
  } catch (error: any) {
    console.error('Error creating teacher:', error);
    return NextResponse.json(
      { error: error.response?.data?.detail || 'Failed to create teacher' },
      { status: error.response?.status || 500 }
    );
  }
}
