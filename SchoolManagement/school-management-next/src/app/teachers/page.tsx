import TeachersClient from './teachers-client';
import { getTeachers } from '@/lib/server/services/teachers';

export default async function TeachersPage() {
  const teachers = await getTeachers();
  return <TeachersClient initialTeachers={teachers} />;
}
