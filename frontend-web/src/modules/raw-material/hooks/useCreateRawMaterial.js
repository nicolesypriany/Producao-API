import { useForm } from "react-hook-form";
import { createRawMaterial as createRawMaterialService } from "../services";

export function useCreateRawMaterial() {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    defaultValues: { name: "", supplier: "", unit: "", price: "" },
  });

  const onSubmit = async (data) => await createRawMaterialService(data);

  return {
    register,
    handleSubmit,
    errors,
    onSubmit,
  };
}
