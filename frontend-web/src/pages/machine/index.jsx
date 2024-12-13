import { Button, Flex, Heading, Input } from "@chakra-ui/react";
import { useForm } from "react-hook-form";
import { Field } from "../../components/ui/field";

const Machine = () => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({ defaultValues: { machineName: "", mark: "" } });

  const onSubmit = (data) => {
    console.log(data);
    fetch("http://example.com/api/machines", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    })
      .then((response) => response.json())
      .then((result) => {
        console.log("Success:", result);
      })
      .catch((error) => {
        console.error("Error:", error);
      });
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <Flex p={4} direction="column" gap={5}>
        <Flex w="100%" justifyContent="center">
          <Heading fontSize="4xl">Cadastro de máquinas</Heading>
        </Flex>

        <Flex direction="column" gap={4}>
          <Field
            label="Nome da máquina"
            invalid={!!errors.machineName}
            errorText={errors.machineName?.message}
          >
            <Input
              placeholder="Digite o nome da máquina"
              {...register("machineName", {
                required: "Nome da máquina é obrigatório",
              })}
            />
          </Field>

          <Field
            label="Marca"
            invalid={!!errors.mark}
            errorText={errors.mark?.message}
          >
            <Input
              placeholder="Digite o nome da marca"
              {...register("mark", {
                required: "Marca é obrigatório",
              })}
            />
          </Field>

          <Button type="submit" w="15%">
            Cadastrar
          </Button>
        </Flex>
      </Flex>
    </form>
  );
};

export default Machine;
