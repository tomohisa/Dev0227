"use client";

import { useState } from "react";
import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { TeacherApi } from "@/lib/api";
import { useRouter } from "next/navigation";

export function AddTeacherDialog() {
  const router = useRouter();
  const [open, setOpen] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  
  const [formData, setFormData] = useState({
    name: "",
    teacherId: "",
    subject: "",
    email: "",
    phoneNumber: "",
    address: "",
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    try {
      setLoading(true);
      setError(null);
      
      // Validate required fields
      if (!formData.name) {
        setError("Name is required");
        setLoading(false);
        return;
      }
      
      if (!formData.teacherId) {
        setError("Teacher ID is required");
        setLoading(false);
        return;
      }
      
      if (!formData.subject) {
        setError("Subject is required");
        setLoading(false);
        return;
      }
      
      if (!formData.email) {
        setError("Email is required");
        setLoading(false);
        return;
      }
      
      if (!formData.phoneNumber) {
        setError("Phone number is required");
        setLoading(false);
        return;
      }
      
      if (!formData.address) {
        setError("Address is required");
        setLoading(false);
        return;
      }
      
      console.log("Submitting teacher data:", {
        name: formData.name,
        teacherId: formData.teacherId,
        subject: formData.subject,
        email: formData.email,
        phoneNumber: formData.phoneNumber,
        address: formData.address,
      });
      
      await TeacherApi.registerTeacher({
        name: formData.name,
        teacherId: formData.teacherId,
        subject: formData.subject,
        email: formData.email,
        phoneNumber: formData.phoneNumber,
        address: formData.address,
      });
      
      setOpen(false);
      setFormData({
        name: "",
        teacherId: "",
        subject: "",
        email: "",
        phoneNumber: "",
        address: "",
      });
      
      // Refresh the page to show the new teacher
      router.refresh();
      
      // Force a page reload to update the data
      window.location.reload();
    } catch (err: any) {
      console.error("Error registering teacher:", err);
      setError(err.response?.data?.detail || "Failed to register teacher. Please try again.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button>Add New Teacher</Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[425px]">
        <form onSubmit={handleSubmit}>
          <DialogHeader>
            <DialogTitle>Add New Teacher</DialogTitle>
            <DialogDescription>
              Enter the teacher details below to register a new teacher.
            </DialogDescription>
          </DialogHeader>
          <div className="grid gap-4 py-4">
            <div className="grid grid-cols-4 items-center gap-4">
              <Label htmlFor="name" className="text-right">
                Name
              </Label>
              <Input
                id="name"
                name="name"
                value={formData.name}
                onChange={handleChange}
                className="col-span-3"
                required
              />
            </div>
            <div className="grid grid-cols-4 items-center gap-4">
              <Label htmlFor="teacherId" className="text-right">
                Teacher ID
              </Label>
              <Input
                id="teacherId"
                name="teacherId"
                value={formData.teacherId}
                onChange={handleChange}
                className="col-span-3"
                required
              />
            </div>
            <div className="grid grid-cols-4 items-center gap-4">
              <Label htmlFor="subject" className="text-right">
                Subject
              </Label>
              <Input
                id="subject"
                name="subject"
                value={formData.subject}
                onChange={handleChange}
                className="col-span-3"
                required
              />
            </div>
            <div className="grid grid-cols-4 items-center gap-4">
              <Label htmlFor="email" className="text-right">
                Email
              </Label>
              <Input
                id="email"
                name="email"
                type="email"
                value={formData.email}
                onChange={handleChange}
                className="col-span-3"
                required
              />
            </div>
            <div className="grid grid-cols-4 items-center gap-4">
              <Label htmlFor="phoneNumber" className="text-right">
                Phone
              </Label>
              <Input
                id="phoneNumber"
                name="phoneNumber"
                value={formData.phoneNumber}
                onChange={handleChange}
                className="col-span-3"
                required
              />
            </div>
            <div className="grid grid-cols-4 items-center gap-4">
              <Label htmlFor="address" className="text-right">
                Address
              </Label>
              <Input
                id="address"
                name="address"
                value={formData.address}
                onChange={handleChange}
                className="col-span-3"
                required
              />
            </div>
          </div>
          {error && (
            <div className="bg-red-50 p-2 rounded-md text-red-500 mb-4">
              <p>{error}</p>
            </div>
          )}
          <DialogFooter>
            <Button type="submit" disabled={loading}>
              {loading ? "Registering..." : "Register Teacher"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
